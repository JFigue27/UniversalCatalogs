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
        protected string CACHE_GET_ALL { get { return typeof(Entity).Name + "_GetAll_"; } }
        protected string CACHE_GET_BY_ID { get { return typeof(Entity).Name + "_GetById_"; } }
        protected string CACHE_GET_PAGED { get { return typeof(Entity).Name + "_GetPaged_"; } }
        protected string CACHE_GET_SINGLE_WHERE { get { return typeof(Entity).Name + "_GetSingleWhere_"; } }

        protected string CACHE_CONTAINER_GET_PAGED { get { return typeof(Entity).Name + "_Container_GetPaged_"; } }
        protected string CACHE_CONTAINER_GET_SINGLE_WHERE { get { return typeof(Entity).Name + "_Container_GetSingleWhere_"; } }

        #region HOOKS
        virtual protected SqlExpression<Entity> OnGetList(SqlExpression<Entity> query) { return query; }
        virtual protected SqlExpression<Entity> OnGetSingle(SqlExpression<Entity> query) { return OnGetList(query); }
        virtual protected IEnumerable<Entity> AdapterOut(params Entity[] entities) { return entities.ToList(); }
        virtual protected List<Entity> BeforePaginate(List<Entity> entities) { return entities; }
        virtual protected bool PopulateForSearch(params Entity[] entities) { return false; } // return true to avoid calling AdapterOut when getPage because they are the same.
        #endregion

        virtual public List<Entity> GetAll()
        {
            var cache = Cache.Get<List<Entity>>(CACHE_GET_ALL);
            if (cache != null)
                return cache;

            var query = OnGetList(Db.From<Entity>());
            var entities = AdapterOut(BeforePaginate(Db.LoadSelect(query)).ToArray());

            var response = entities.ToList();
            Cache.Set(CACHE_GET_ALL, response);
            return response;
        }

        virtual public async Task<List<Entity>> GetAllAsync()
        {
            var cache = Cache.Get<List<Entity>>(CACHE_GET_ALL);
            if (cache != null)
                return cache;

            var query = OnGetList(Db.From<Entity>());
            var entities = AdapterOut(BeforePaginate(await Db.LoadSelectAsync(query)).ToArray());

            var response = entities.ToList();
            Cache.Set(CACHE_GET_ALL, response);
            return response;
        }

        virtual public Entity GetById(long id)
        {
            var cacheKey = CACHE_GET_BY_ID + id;

            var cache = Cache.Get<Entity>(cacheKey);
            if (cache != null)
                return cache;

            var query = OnGetSingle(Db.From<Entity>())
                    .Where(e => e.Id == id);

            var entity = Db.LoadSelect(query).FirstOrDefault();

            AdapterOut(entity);

            var response = entity;
            Cache.Set(cacheKey, response);
            return response;
        }

        virtual public async Task<Entity> GetByIdAsync(long id)
        {
            var cacheKey = CACHE_GET_BY_ID + id;

            var cache = Cache.Get<Entity>(cacheKey);
            if (cache != null)
                return cache;

            var query = OnGetSingle(Db.From<Entity>())
                    .Where(e => e.Id == id);

            var entity = (await Db.LoadSelectAsync(query)).FirstOrDefault();

            var response = entity;
            Cache.Set(cacheKey, response);
            return response;
        }

        virtual public CommonResponse GetPaged(int perPage, int page, string filterGeneral, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, CommonResponse>>(CACHE_CONTAINER_GET_PAGED);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, CommonResponse>();
            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CACHE_GET_PAGED;
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
                    cacheKey += $"_{queryParam}_{queryParamValue}";

                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);

                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    if (tProp == typeof(string))
                        query.Where($"{query.SqlColumn(sPropertyName)} like '%{value.Value}%'");
                    else
                        query.Where($"{query.SqlColumn(sPropertyName)} = {value.Value}");
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

            var entities = Db.LoadSelect(query).ToArray();
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
                        filteredResultSet.Add(e);
                }

                //DID NOT WORK SOMETIMES:
                //resultset = resultset.Where(e => searchableProps.Any(prop =>
                //                                    arrFilterGeneral.All(keyword =>
                //                                        ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                //                                        .Contains(keyword))));
            }
            else
                filteredResultSet = new HashSet<Entity>(entities);

            filterResponse.total_filtered_items = filteredResultSet.Count();
            #endregion

            #region Pagination
            IEnumerable<Entity> afterPaginate;
            if (perPage != 0)
            {
                var totalPagesCount = (filterResponse.total_filtered_items + perPage - 1) / perPage;
                if (page > totalPagesCount)
                    page = totalPagesCount;

                afterPaginate = BeforePaginate(filteredResultSet.ToList()).Skip((page - 1) * perPage).Take(perPage);
                filterResponse.page = page;
            }
            else
                afterPaginate = BeforePaginate(filteredResultSet.ToList());
            #endregion

            #region AdapterOut Hook
            if (!PopulateForSearchEqualsAdapterOut)
                AdapterOut(afterPaginate.ToArray());
            #endregion

            var response = new CommonResponse { Result = afterPaginate, AdditionalData = filterResponse };
            cacheContainer[cacheKey] = response;
            Cache.Replace(CACHE_CONTAINER_GET_PAGED, cacheContainer);
            return response;
        }

        virtual public async Task<CommonResponse> GetPagedAsync(int perPage, int page, string filterGeneral, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, CommonResponse>>(CACHE_CONTAINER_GET_PAGED);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, CommonResponse>();
            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CACHE_GET_PAGED;
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
                    cacheKey += $"_{queryParam}_{queryParamValue}";

                    string sPropertyName = queryParam;

                    PropertyInfo oProp = typeof(Entity).GetProperty(sPropertyName);
                    if (oProp == null) continue; //Ignore non-existing properties, they could be just different query parameters.


                    Type tProp = oProp.PropertyType;
                    //Nullable properties have to be treated differently, since we
                    //  use their underlying property to set the value in the object
                    if (tProp.IsGenericType
                        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        //Get the underlying type property instead of the nullable generic
                        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;

                    ParameterExpression entityParameter = Expression.Parameter(typeof(Entity), "entityParameter");
                    Expression childProperty = Expression.PropertyOrField(entityParameter, sPropertyName);

                    var value = Expression.Constant(Convert.ChangeType(queryParamValue, tProp));

                    if (tProp == typeof(string))
                        query.Where($"{query.SqlColumn(sPropertyName)} like '%{value.Value}%'");
                    else
                        query.Where($"{query.SqlColumn(sPropertyName)} = {value.Value}");
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

            var entities = (await Db.LoadSelectAsync(query)).ToArray();

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
                        filteredResultSet.Add(e);
                }

                //DID NOT WORK SOMETIMES:
                //resultset = resultset.Where(e => searchableProps.Any(prop =>
                //                                    arrFilterGeneral.All(keyword =>
                //                                        ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                //                                        .Contains(keyword))));
            }
            else
                filteredResultSet = new HashSet<Entity>(entities);

            filterResponse.total_filtered_items = filteredResultSet.Count();
            #endregion

            #region Pagination
            IEnumerable<Entity> afterPaginate;
            if (perPage != 0)
                afterPaginate = BeforePaginate(filteredResultSet.ToList()).Skip((page - 1) * perPage).Take(perPage);
            else
                afterPaginate = BeforePaginate(filteredResultSet.ToList());
            #endregion

            #region AdapterOut Hook
            if (!PopulateForSearchEqualsAdapterOut)
                AdapterOut(afterPaginate.ToArray());
            #endregion

            var response = new CommonResponse { Result = afterPaginate.ToList(), AdditionalData = filterResponse };
            cacheContainer[cacheKey] = response;
            Cache.Replace(CACHE_CONTAINER_GET_PAGED, cacheContainer);
            return response;
        }

        virtual public Entity GetSingleWhere(string Property, object Value, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, Entity>>(CACHE_CONTAINER_GET_SINGLE_WHERE);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, Entity>();

            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CACHE_GET_SINGLE_WHERE;
            cacheKey += $"_{ Property}_{Value}";

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

            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    cacheKey += $"_{queryParam}={queryParamValue}";
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

                    query.And(lambda);
                }
            }

            cacheContainer.TryGetValue(cacheKey, out Entity cache);
            if (cache != null) return cache;

            query = OnGetSingle(query);

            var entity = Db.LoadSelect(query).FirstOrDefault();
            if (entity != null) AdapterOut(entity);

            var response = entity;
            cacheContainer[cacheKey] = response;
            Cache.Replace(CACHE_CONTAINER_GET_SINGLE_WHERE, cacheContainer);
            return response;
        }

        virtual public async Task<Entity> GetSingleWhereAsync(string Property, object Value, SqlExpression<Entity> query = null, string cacheKey = null)
        {
            var cacheContainer = Cache.Get<Dictionary<string, Entity>>(CACHE_CONTAINER_GET_SINGLE_WHERE);
            if (cacheContainer == null) cacheContainer = new Dictionary<string, Entity>();

            if (string.IsNullOrWhiteSpace(cacheKey)) cacheKey = CACHE_GET_SINGLE_WHERE;
            cacheKey += $"_{ Property}_{Value}";

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

            foreach (var queryParam in Request.QueryString.AllKeys)
            {
                string queryParamValue = Request.QueryString[queryParam];
                if (IsValidParam(queryParam) && IsValidJSValue(queryParamValue))
                {
                    cacheKey += $"_{queryParam}={queryParamValue}";
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

                    query.And(lambda);
                }
            }

            cacheContainer.TryGetValue(cacheKey, out Entity cache);
            if (cache != null) return cache;

            query = OnGetSingle(query);

            var entity = (await Db.LoadSelectAsync(query)).FirstOrDefault();
            if (entity != null) AdapterOut(entity);

            var response = entity;
            cacheContainer[cacheKey] = response;
            Cache.Replace(CACHE_CONTAINER_GET_SINGLE_WHERE, cacheContainer);
            return response;
        }
    }
}
