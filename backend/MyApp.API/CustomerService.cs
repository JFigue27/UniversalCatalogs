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
    public class CustomerService : BaseService<CustomerLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllCustomers request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetCustomerById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetCustomerWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedCustomers request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateCustomerInstance request)
        {
            return WithDb(db =>
            {
                var entity = request.ConvertTo<Customer>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
            return InTransaction(db =>
            {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
            return InTransaction(db =>
            {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteCustomer request)
        {
            var entity = request.ConvertTo<Customer>();
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
