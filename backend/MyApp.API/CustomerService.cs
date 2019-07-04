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
    public class CustomerService : Service
    {
        public ICustomerLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllCustomers request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetCustomerById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetCustomerWhere request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedCustomers request)
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
        public object Post(CreateCustomerInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Customer>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
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
    [Route("/Customer", "GET")]
    public class GetAllCustomers : GetAll<Customer> { }

    [Route("/Customer/{Id}", "GET")]
    public class GetCustomerById : GetSingleById<Customer> { }

    [Route("/Customer/GetSingleWhere", "GET")]
    [Route("/Customer/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetCustomerWhere : GetSingleWhere<Customer> { }

    [Route("/Customer/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedCustomers : GetPaged<Customer> { }
    #endregion

    #region Generic Write
    [Route("/Customer/CreateInstance", "POST")]
    public class CreateCustomerInstance : Customer { }

    [Route("/Customer", "POST")]
    public class InsertCustomer : Customer { }

    [Route("/Customer", "PUT")]
    public class UpdateCustomer : Customer { }

    [Route("/Customer", "DELETE")]
    [Route("/Customer/{Id}", "DELETE")]
    public class DeleteCustomer : Customer { }
    #endregion
}
