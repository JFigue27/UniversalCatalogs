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
    public class ShiftService : BaseService<ShiftLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllShifts request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetShiftById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetShiftWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedShifts request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateShiftInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Shift>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertShift request)
        {
            var entity = request.ConvertTo<Shift>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateShift request)
        {
            var entity = request.ConvertTo<Shift>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteShift request)
        {
            var entity = request.ConvertTo<Shift>();
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
    [Route("/Shift", "GET")]
    public class GetAllShifts : GetAll<Shift> { }

    [Route("/Shift/{Id}", "GET")]
    public class GetShiftById : GetSingleById<Shift> { }

    [Route("/Shift/GetSingleWhere", "GET")]
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

    [Route("/Shift", "DELETE")]
    [Route("/Shift/{Id}", "DELETE")]
    public class DeleteShift : Shift { }
    #endregion
}
