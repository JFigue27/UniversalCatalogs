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
    // [Authenticate]
    public class EmployeeService : Service
    {
        public IEmployeeLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllEmployees request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetEmployeeById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetEmployeeWhere request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedEmployees request)
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
        public object Post(CreateEmployeeInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Employee>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteEmployee request)
        {
            var entity = request.ConvertTo<Employee>();
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
