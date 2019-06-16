using Reusable.CRUD.Contract;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Reusable.CRUD.Implementations.SS
{
    public abstract class ReadOnlyLogic<Entity> : BaseLogic, ILogicReadOnly<Entity>, ILogicReadOnlyAsync<Entity> where Entity : BaseEntity, new()
    {
        protected string CacheGetAllKey { get { return typeof(Entity).Name + "_GetAll"; } }
        protected string CacheGetByIdKey { get { return typeof(Entity).Name + "_GetById"; } }
        protected string CacheGetPagedKey { get { return typeof(Entity).Name + "_GetPaged"; } }
        protected string CacheGetSingleWhereKey { get { return typeof(Entity).Name + "_GetSingleWhere"; } }

        protected string CacheContainerGetPagedKey { get { return typeof(Entity).Name + "_Container_GetPaged"; } }
        protected string CacheContainerGetSingleWhereKey { get { return typeof(Entity).Name + "_ContainerGetSingleWhere"; } }

        #region HOOKS
        virtual protected SqlExpression<Entity> OnGetList(SqlExpression<Entity> query) { return query; }
        virtual protected SqlExpression<Entity> OnGetSingle(SqlExpression<Entity> query) { return OnGetList(query); }
        virtual protected IEnumerable<Entity> AdapterOut(params Entity[] entities) { return entities.ToList(); }
        virtual protected bool PopulateForSearch(params Entity[] entities) { return false; } // return true to avoid calling AdapterOut when getPage because they are the same.
        #endregion

        virtual public List<Entity> GetAll()
        {
            var cache = Cache.Get<List<Entity>>(CacheGetAllKey);
            if (cache != null)
                return cache;

            var query = OnGetList(Db.From<Entity>());
            var entities = AdapterOut(Db.Select(query).ToArray());

            var response = entities.ToList();
            Cache.Set(CacheGetAllKey, response);
            return response;
        }

        virtual public async Task<List<Entity>> GetAllAsync()
        {
            var cache = Cache.Get<List<Entity>>(CacheGetAllKey);
            if (cache != null)
                return cache;

            var query = OnGetList(Db.From<Entity>());
            var entities = AdapterOut((await Db.SelectAsync(query)).ToArray());

            var response = entities.ToList();
            Cache.Set(CacheGetAllKey, response);
            return response;
        }

        virtual public Entity GetById(long id)
        {
            var cacheKey = CacheGetByIdKey + "_" + id;

            var cache = Cache.Get<Entity>(cacheKey);
            if (cache != null)
                return cache;

            var query = OnGetSingle(Db.From<Entity>())
                    .Where(e => e.Id == id);

            var entity = Db.Single(query);

            AdapterOut(entity);

            var response = entity;
            Cache.Set(cacheKey, response);
            return response;
        }

        virtual public async Task<Entity> GetByIdAsync(long id)
        {
            var cacheKey = CacheGetByIdKey + "_" + id;

            var cache = Cache.Get<Entity>(cacheKey);
            if (cache != null)
                return cache;

            var query = OnGetSingle(Db.From<Entity>())
                    .Where(e => e.Id == id);

            var entity = await Db.SingleAsync(query);

            var response = entity;
            Cache.Set(cacheKey, response);
            return response;
        }

        virtual public CommonResponse GetPaged(int perPage, int page, string filterGeneral, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, CommonResponse>>(CacheContainerGetPagedKey);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, CommonResponse>();
            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CacheGetPagedKey;
            cacheKey += $"_{ perPage}_{page}_{filterGeneral}";

            if (query == null) query = Db.From<Entity>();

            var filterResponse = new FilterResponse();

            #region Filter by User
            //if (request.FilterUser.HasValue && typeof(Entity).IsSubclassOf(typeof(BaseDocument)))
            //{
            //    query.LeftJoin<Track>()
            //        .LeftJoin<Track, User>((t, u) => t.User_AssignedToKey == u.Id)
            //        .Where<User>(u => u.Id == request.FilterUser);
            //}
            #endregion

            #region Apply Database Filtering
            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                    }

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    // let's perform the conversion only if we really need it
                    var converted = value.Type != childProperty.Type
                        ? Expression.Convert(value, childProperty.Type)
                        : (Expression)value;

                    Expression<Func<Entity, bool>> lambda;
                    if (tProp == typeof(String))
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                    }
                    else
                    {
                        Expression comparison = Expression.Equal(childProperty, converted);
                        lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                    }

                    cacheKey += "_" + lambda.ToString();

                    query.And(lambda);
                }
            }
            #endregion

            #region From Cache
            cacheContainer.TryGetValue(cacheKey, out CommonResponse cache);
            if (cache != null) return cache;
            #endregion

            #region OnGetList Hook
            query = OnGetList(query);
            #endregion

            #region Non-Database Parameters Filters

            #endregion

            var entities = Db.Select(query).ToArray();
            filterResponse.total_items = entities.Count();

            #region Apply General Search Filter

            bool PopulateForSearchEqualsAdapterOut = false;
            PopulateForSearchEqualsAdapterOut = PopulateForSearch(entities);

            HashSet<Entity> filteredResultSet = new HashSet<Entity>();
            if (!string.IsNullOrWhiteSpace(filterGeneral))
            {
                string[] arrFilterGeneral = filterGeneral.ToLower().Split(' ');

                var searchableProps = typeof(Entity).GetProperties().Where(prop => new[] { "String" }.Contains(prop.PropertyType.Name));

                foreach (var e in entities)
                {
                    bool bAllKeywordsFound = true;
                    foreach (var keyword in arrFilterGeneral)
                    {
                        bool bAtLeastOnePropertyContainsIt = false;
                        foreach (var prop in searchableProps)
                        {
                            string a = (string)prop.GetValue(e, null);
                            if (a != null && a.ToLower().Contains(keyword.Trim()))
                            {
                                bAtLeastOnePropertyContainsIt = true;
                                break;
                            }
                        }
                        if (!bAtLeastOnePropertyContainsIt)
                        {
                            bAllKeywordsFound = false;
                            break;
                        }
                    }
                    if (bAllKeywordsFound)
                    {
                        filteredResultSet.Add(e);
                    }
                }

                //DID NOT WORK SOMETIMES:
                //resultset = resultset.Where(e => searchableProps.Any(prop =>
                //                                    arrFilterGeneral.All(keyword =>
                //                                        ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                //                                        .Contains(keyword))));
            }
            else
            {
                filteredResultSet = new HashSet<Entity>(entities);
            }

            filterResponse.total_filtered_items = filteredResultSet.Count();
            #endregion

            #region Pagination
            IEnumerable<Entity> afterPaginate;
            if (perPage != 0)
            {
                afterPaginate = filteredResultSet.Skip((page - 1) * perPage).Take(perPage);
            }
            else
            {
                afterPaginate = filteredResultSet;
            }
            #endregion

            #region AdapterOut Hook
            if (!PopulateForSearchEqualsAdapterOut)
            {
                AdapterOut(afterPaginate.ToArray());
            }
            #endregion

            var response = new CommonResponse { Result = afterPaginate, AdditionalData = filterResponse };
            cacheContainer[cacheKey] = response;
            Cache.Replace(CacheContainerGetPagedKey, cacheContainer);
            return response;
        }

        virtual public async Task<CommonResponse> GetPagedAsync(int perPage, int page, string filterGeneral, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, CommonResponse>>(CacheContainerGetPagedKey);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, CommonResponse>();
            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CacheGetPagedKey;
            cacheKey += $"_{ perPage}_{page}_{filterGeneral}";

            if (query == null) query = Db.From<Entity>();

            var filterResponse = new FilterResponse();

            #region Filter by User
            //if (request.FilterUser.HasValue && typeof(Entity).IsSubclassOf(typeof(BaseDocument)))
            //{
            //    query.LeftJoin<Track>()
            //        .LeftJoin<Track, User>((t, u) => t.User_AssignedToKey == u.Id)
            //        .Where<User>(u => u.Id == request.FilterUser);
            //}
            #endregion

            #region Apply Database Filtering
            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                    }

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    // let's perform the conversion only if we really need it
                    var converted = value.Type != childProperty.Type
                        ? Expression.Convert(value, childProperty.Type)
                        : (Expression)value;

                    Expression<Func<Entity, bool>> lambda;
                    if (tProp == typeof(String))
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                    }
                    else
                    {
                        Expression comparison = Expression.Equal(childProperty, converted);
                        lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                    }

                    cacheKey += "_" + lambda.ToString();

                    query.And(lambda);
                }
            }

            #endregion

            #region From Cache
            cacheContainer.TryGetValue(cacheKey, out CommonResponse cache);
            if (cache != null) return cache;
            #endregion

            #region OnGetList Hook
            query = OnGetList(query);
            #endregion

            #region Non-Database Parameters Filters

            #endregion

            var entities = (await Db.SelectAsync(query)).ToArray();
            filterResponse.total_items = entities.Count();

            #region Apply General Search Filter

            bool PopulateForSearchEqualsAdapterOut = false;
            PopulateForSearchEqualsAdapterOut = PopulateForSearch(entities);

            HashSet<Entity> filteredResultSet = new HashSet<Entity>();
            if (!string.IsNullOrWhiteSpace(filterGeneral))
            {
                string[] arrFilterGeneral = filterGeneral.ToLower().Split(' ');

                var searchableProps = typeof(Entity).GetProperties().Where(prop => new[] { "String" }.Contains(prop.PropertyType.Name));

                foreach (var e in entities)
                {
                    bool bAllKeywordsFound = true;
                    foreach (var keyword in arrFilterGeneral)
                    {
                        bool bAtLeastOnePropertyContainsIt = false;
                        foreach (var prop in searchableProps)
                        {
                            string a = (string)prop.GetValue(e, null);
                            if (a != null && a.ToLower().Contains(keyword.Trim()))
                            {
                                bAtLeastOnePropertyContainsIt = true;
                                break;
                            }
                        }
                        if (!bAtLeastOnePropertyContainsIt)
                        {
                            bAllKeywordsFound = false;
                            break;
                        }
                    }
                    if (bAllKeywordsFound)
                    {
                        filteredResultSet.Add(e);
                    }
                }

                //DID NOT WORK SOMETIMES:
                //resultset = resultset.Where(e => searchableProps.Any(prop =>
                //                                    arrFilterGeneral.All(keyword =>
                //                                        ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                //                                        .Contains(keyword))));
            }
            else
            {
                filteredResultSet = new HashSet<Entity>(entities);
            }

            filterResponse.total_filtered_items = filteredResultSet.Count();
            #endregion

            #region Pagination
            IEnumerable<Entity> afterPaginate;
            if (perPage != 0)
            {
                afterPaginate = filteredResultSet.Skip((page - 1) * perPage).Take(perPage);
            }
            else
            {
                afterPaginate = filteredResultSet;
            }
            #endregion

            #region AdapterOut Hook
            if (!PopulateForSearchEqualsAdapterOut)
            {
                AdapterOut(afterPaginate.ToArray());
            }
            #endregion

            var response = new CommonResponse { Result = afterPaginate.ToList(), AdditionalData = filterResponse };
            cacheContainer[cacheKey] = response;
            Cache.Replace(CacheContainerGetPagedKey, cacheContainer);
            return response;
        }

        virtual public Entity GetSingleWhere(string Property, object Value, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, Entity>>(CacheContainerGetSingleWhereKey);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, Entity>();

            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CacheGetSingleWhereKey;
            cacheKey += $"_{ Property}_{Value}";

            cacheContainer.TryGetValue(cacheKey, out Entity cache);
            if (cache != null) return cache;

            if (query == null) query = Db.From<Entity>();

            if (!string.IsNullOrWhiteSpace(Property) && Value != null)
            {
                string sPropertyName = Property;

                PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                Type tProp = oProp.PropertyType;
                //Nullable properties have to be treated differently, since we
                //  use their underlying property to set the value in the object
                if (tProp.IsGenericType
                    && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //Get the underlying type property instead of the nullable generic
                    tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                }

                ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                var value = Expression.Constant(Convert.ChangeType(Value, tProp));

                // let's perform the conversion only if we really need it
                var converted = value.Type != childProperty.Type
                    ? Expression.Convert(value, childProperty.Type)
                    : (Expression)value;

                Expression<Func<Entity, bool>> lambda;
                if (tProp == typeof(String))
                {
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                }
                else
                {
                    Expression comparison = Expression.Equal(childProperty, converted);
                    lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                }

                query.And(lambda);
            }

            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                    }

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    // let's perform the conversion only if we really need it
                    var converted = value.Type != childProperty.Type
                        ? Expression.Convert(value, childProperty.Type)
                        : (Expression)value;

                    Expression<Func<Entity, bool>> lambda;
                    if (tProp == typeof(String))
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                    }
                    else
                    {
                        Expression comparison = Expression.Equal(childProperty, converted);
                        lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                    }

                    cacheKey += "_" + lambda.ToString();

                    query.And(lambda);
                }
            }

            query = OnGetSingle(query);

            var entity = Db.Single(query);
            if (entity != null) AdapterOut(entity);

            var response = entity;
            cacheContainer[cacheKey] = response;
            Cache.Replace(CacheContainerGetSingleWhereKey, cacheContainer);
            return response;
        }

        virtual public async Task<Entity> GetSingleWhereAsync(string Property, object Value, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, Entity>>(CacheContainerGetSingleWhereKey);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, Entity>();

            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CacheGetSingleWhereKey;
            cacheKey += $"_{ Property}_{Value}";

            cacheContainer.TryGetValue(cacheKey, out Entity cache);
            if (cache != null) return cache;

            if (query == null) query = Db.From<Entity>();

            if (!string.IsNullOrWhiteSpace(Property) && Value != null)
            {
                string sPropertyName = Property;

                PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                Type tProp = oProp.PropertyType;
                //Nullable properties have to be treated differently, since we
                //  use their underlying property to set the value in the object
                if (tProp.IsGenericType
                    && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //Get the underlying type property instead of the nullable generic
                    tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                }

                var value = Expression.Constant(Convert.ChangeType(Value, tProp));

                if (tProp == typeof(string))
                {
                    query.Where($"{query.SqlColumn(Property)} like '%{value.Value}%'");
                }
                else
                {
                    query.Where($"{query.SqlColumn(Property)} = {value.Value}");
                }
            }

            query = OnGetSingle(query);

            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                    }

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);


                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    // let's perform the conversion only if we really need it
                    var converted = value.Type != childProperty.Type
                        ? Expression.Convert(value, childProperty.Type)
                        : (Expression)value;

                    Expression<Func<Entity, bool>> lambda;
                    if (tProp == typeof(String))
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        lambda = Expression.Lambda<Func<Entity, bool>>(Expression.Call(converted, method, childProperty), entityParameter);
                    }
                    else
                    {
                        Expression comparison = Expression.Equal(childProperty, converted);
                        lambda = Expression.Lambda<Func<Entity, bool>>(comparison, entityParameter);
                    }

                    cacheKey += "_" + lambda.ToString();

                    query.And(lambda);
                }
            }

            var entity = await Db.SingleAsync(query);

            if (entity != null) AdapterOut(entity);

            var response = entity;
            cacheContainer[cacheKey] = response;
            Cache.Replace(CacheContainerGetSingleWhereKey, cacheContainer);
            return response;
        }

        protected class FilterResponse
        {
            public List<List<BaseEntity>> Dropdowns { get; set; }
            public int total_items { get; set; }
            public int total_filtered_items { get; set; }
        }

        protected bool IsValidJSValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null" || value == "undefined")
            {
                return false;
            }

            return true;
        }

        public Exception GetOriginalException(Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return GetOriginalException(ex.InnerException);
        }

        protected bool IsValidParam(string param)
        {
            //reserved and invalid params:
            if (new string[] {
                "limit",
                "perPage",
                "page",
                "filterGeneral",
                "itemsCount",
                "noCache",
                "totalItems",
                "parentKey",
                "parentField",
                "filterUser"
            }.Contains(param))
                return false;

            return true;
        }
    }
}
