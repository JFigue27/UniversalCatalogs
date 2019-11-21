using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;



namespace MyApp.API
{
    // [Authenticate]
    public class CatalogTypeFieldService : BaseService<CatalogTypeFieldLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllCatalogTypeFields request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetCatalogTypeFieldById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetCatalogTypeFieldWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedCatalogTypeFields request)
        {
            var query = AutoQuery.CreateQuery(request, Request);

            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral,
                query,
                requiresKeysInJsons: request.RequiresKeysInJsons
                ));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateCatalogTypeFieldInstance request)
        {
            return WithDb(db =>
            {
                var entity = request.ConvertTo<CatalogTypeField>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertCatalogTypeField request)
        {
            var entity = request.ConvertTo<CatalogTypeField>();
            return InTransaction(db =>
            {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateCatalogTypeField request)
        {
            var entity = request.ConvertTo<CatalogTypeField>();
            return InTransaction(db =>
            {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteCatalogTypeField request)
        {
            var entity = request.ConvertTo<CatalogTypeField>();
            return InTransaction(db =>
            {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        public object Delete(DeleteByIdCatalogTypeField request)
        {
            var entity = request.ConvertTo<CatalogTypeField>();
            return InTransaction(db =>
            {
                Logic.RemoveById(entity.Id);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Specific
        
        
        #endregion
    }

    #region Specific
    
    
    #endregion

    #region Generic Read Only
    [Route("/CatalogTypeField", "GET")]
    public class GetAllCatalogTypeFields : GetAll<CatalogTypeField> { }

    [Route("/CatalogTypeField/{Id}", "GET")]
    public class GetCatalogTypeFieldById : GetSingleById<CatalogTypeField> { }

    [Route("/CatalogTypeField/GetSingleWhere", "GET")]
    [Route("/CatalogTypeField/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetCatalogTypeFieldWhere : GetSingleWhere<CatalogTypeField> { }

    [Route("/CatalogTypeField/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedCatalogTypeFields : QueryDb<CatalogTypeField> {
        public string FilterGeneral { get; set; }
        //public long? FilterUser { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }

        public bool RequiresKeysInJsons { get; set; }
    }
    #endregion

    #region Generic Write
    [Route("/CatalogTypeField/CreateInstance", "POST")]
    public class CreateCatalogTypeFieldInstance : CatalogTypeField { }

    [Route("/CatalogTypeField", "POST")]
    public class InsertCatalogTypeField : CatalogTypeField { }

    [Route("/CatalogTypeField", "PUT")]
    public class UpdateCatalogTypeField : CatalogTypeField { }

    [Route("/CatalogTypeField", "DELETE")]
    public class DeleteCatalogTypeField : CatalogTypeField { }

    [Route("/CatalogTypeField/{Id}", "DELETE")]
    public class DeleteByIdCatalogTypeField : CatalogTypeField { }
    #endregion
}
