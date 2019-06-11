using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace MyApp.API
{
    [Authenticate]
    public class WorkstationService : Service
    {
        public IWorkstationLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllWorkstations request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetWorkstationById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetWorkstationWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedWorkstations request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetPagedAsync(
                request.Limit,
                request.Page,
                request.FilterGeneral,
                null,
                null);
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateWorkstationInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Workstation>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteWorkstation request)
        {
            var entity = request.ConvertTo<Workstation>();
            InTransaction(() => Logic.Remove(entity));
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion

        private void InTransaction(params Action[] Operations)
        {
            Logic.SetDb(Db);
            Logic.SetAuth(GetSession());
            using (var transaction = Db.OpenTransaction())
            {
                try
                {
                    foreach (var operation in Operations)
                    {
                        operation();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    #region Specific
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/Workstation", "GET")]
    public class GetAllWorkstations : GetAll<Workstation> { }

    [Route("/Workstation/{Id}", "GET")]
    public class GetWorkstationById : GetSingleById<Workstation> { }

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

    [Route("/Workstation/{Id}", "DELETE")]
    public class DeleteWorkstation : Workstation { }
    #endregion
}
