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
    public class AreaService : BaseService<AreaLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllAreas request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetAreaById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetAreaWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedAreas request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateAreaInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Area>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertArea request)
        {
            var entity = request.ConvertTo<Area>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateArea request)
        {
            var entity = request.ConvertTo<Area>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteArea request)
        {
            var entity = request.ConvertTo<Area>();
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
    [Route("/Area", "GET")]
    public class GetAllAreas : GetAll<Area> { }

    [Route("/Area/{Id}", "GET")]
    public class GetAreaById : GetSingleById<Area> { }

    [Route("/Area/GetSingleWhere", "GET")]
    [Route("/Area/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetAreaWhere : GetSingleWhere<Area> { }

    [Route("/Area/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedAreas : GetPaged<Area> { }
    #endregion

    #region Generic Write
    [Route("/Area/CreateInstance", "POST")]
    public class CreateAreaInstance : Area { }

    [Route("/Area", "POST")]
    public class InsertArea : Area { }

    [Route("/Area", "PUT")]
    public class UpdateArea : Area { }

    [Route("/Area", "DELETE")]
    [Route("/Area/{Id}", "DELETE")]
    public class DeleteArea : Area { }
    #endregion
}
