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
    public class EmployeeService : BaseService<EmployeeLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllEmployees request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetEmployeeById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetEmployeeWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedEmployees request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateEmployeeInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Employee>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
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
    [Route("/Employee", "GET")]
    public class GetAllEmployees : GetAll<Employee> { }

    [Route("/Employee/{Id}", "GET")]
    public class GetEmployeeById : GetSingleById<Employee> { }

    [Route("/Employee/GetSingleWhere", "GET")]
    [Route("/Employee/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetEmployeeWhere : GetSingleWhere<Employee> { }

    [Route("/Employee/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedEmployees : GetPaged<Employee> { }
    #endregion

    #region Generic Write
    [Route("/Employee/CreateInstance", "POST")]
    public class CreateEmployeeInstance : Employee { }

    [Route("/Employee", "POST")]
    public class InsertEmployee : Employee { }

    [Route("/Employee", "PUT")]
    public class UpdateEmployee : Employee { }

    [Route("/Employee", "DELETE")]
    [Route("/Employee/{Id}", "DELETE")]
    public class DeleteEmployee : Employee { }
    #endregion
}
