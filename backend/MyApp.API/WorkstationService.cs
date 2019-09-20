using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;

///start:slot:imports<<<///end:slot:imports<<<

namespace MyApp.API
{
    // [Authenticate]
    public class WorkstationService : BaseService<WorkstationLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllWorkstations request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetWorkstationById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetWorkstationWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedWorkstations request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateWorkstationInstance request)
        {
            return WithDb(db =>
            {
                var entity = request.ConvertTo<Workstation>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            return InTransaction(db =>
            {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            return InTransaction(db =>
            {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            return InTransaction(db =>
            {
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
    [Route("/Workstation", "GET")]
    public class GetAllWorkstations : GetAll<Workstation> { }

    [Route("/Workstation/{Id}", "GET")]
    public class GetWorkstationById : GetSingleById<Workstation> { }

    [Route("/Workstation/GetSingleWhere", "GET")]
    [Route("/Workstation/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetWorkstationWhere : GetSingleWhere<Workstation> { }

    [Route("/Workstation/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedWorkstations : GetPaged<Workstation> { }
    #endregion

    #region Generic Write
    [Route("/Workstation/CreateInstance", "POST")]
    public class CreateWorkstationInstance : Workstation { }

    [Route("/Workstation", "POST")]
    public class InsertWorkstation : Workstation { }

    [Route("/Workstation", "PUT")]
    public class UpdateWorkstation : Workstation { }

    [Route("/Workstation", "DELETE")]
    [Route("/Workstation/{Id}", "DELETE")]
    public class DeleteWorkstation : Workstation { }
    #endregion
}
