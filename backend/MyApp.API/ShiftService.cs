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
    public class ShiftService : Service
    {
        public IShiftLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllShifts request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetShiftById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetShiftWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedShifts request)
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
        public object Post(CreateShiftInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Shift>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertShift request)
        {
            var entity = request.ConvertTo<Shift>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateShift request)
        {
            var entity = request.ConvertTo<Shift>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteShift request)
        {
            var entity = request.ConvertTo<Shift>();
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
    [Route("/Shift", "GET")]
    public class GetAllShifts : GetAll<Shift> { }

    [Route("/Shift/{Id}", "GET")]
    public class GetShiftById : GetSingleById<Shift> { }

    [Route("/Shift/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetShiftWhere : GetSingleWhere<Shift> { }

    [Route("/Shift/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedShifts : GetPaged<Shift> { }
    #endregion

    #region Generic Write
    [Route("/Shift/CreateInstance", "POST")]
    public class CreateShiftInstance : Shift { }

    [Route("/Shift", "POST")]
    public class InsertShift : Shift { }

    [Route("/Shift", "PUT")]
    public class UpdateShift : Shift { }

    [Route("/Shift/{Id}", "DELETE")]
    public class DeleteShift : Shift { }
    #endregion
}
