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
    public class CatalogService : BaseService<CatalogLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllCatalogs request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetCatalogById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetCatalogWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedCatalogs request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateCatalogInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Catalog>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertCatalog request)
        {
            var entity = request.ConvertTo<Catalog>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateCatalog request)
        {
            var entity = request.ConvertTo<Catalog>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteCatalog request)
        {
            var entity = request.ConvertTo<Catalog>();
            return InTransaction(db => {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Specific
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion
    }

    #region Specific
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/Catalog", "GET")]
    public class GetAllCatalogs : GetAll<Catalog> { }

    [Route("/Catalog/{Id}", "GET")]
    public class GetCatalogById : GetSingleById<Catalog> { }

    [Route("/Catalog/GetSingleWhere", "GET")]
    [Route("/Catalog/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetCatalogWhere : GetSingleWhere<Catalog> { }

    [Route("/Catalog/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedCatalogs : GetPaged<Catalog> { }
    #endregion

    #region Generic Write
    [Route("/Catalog/CreateInstance", "POST")]
    public class CreateCatalogInstance : Catalog { }

    [Route("/Catalog", "POST")]
    public class InsertCatalog : Catalog { }

    [Route("/Catalog", "PUT")]
    public class UpdateCatalog : Catalog { }

    [Route("/Catalog", "DELETE")]
    [Route("/Catalog/{Id}", "DELETE")]
    public class DeleteCatalog : Catalog { }
    #endregion
}
