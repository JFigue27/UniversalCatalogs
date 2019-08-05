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
    public class UserService : BaseService<UserLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllUsers request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetUserById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetUserWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedUsers request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateUserInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<User>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Generic Document
        virtual public object Post(FinalizeUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Finalize(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(UnfinalizeUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Unfinalize(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(MakeUserRevision request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.MakeRevision(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CheckoutUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Checkout(entity.Id);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CancelCheckoutUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.CancelCheckout(entity.Id);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CheckinUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.Checkin(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CreateAndCheckoutUser request)
        {
            var entity = request.ConvertTo<User>();
            return InTransaction(db => {
                Logic.CreateAndCheckout(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
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
    [Route("/User", "GET")]
    public class GetAllUsers : GetAll<User> { }

    [Route("/User/{Id}", "GET")]
    public class GetUserById : GetSingleById<User> { }

    [Route("/User/GetSingleWhere", "GET")]
    [Route("/User/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetUserWhere : GetSingleWhere<User> { }

    [Route("/User/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedUsers : GetPaged<User> { }
    #endregion

    #region Generic Write
    [Route("/User/CreateInstance", "POST")]
    public class CreateUserInstance : User { }

    [Route("/User", "POST")]
    public class InsertUser : User { }

    [Route("/User", "PUT")]
    public class UpdateUser : User { }

    [Route("/User", "DELETE")]
    [Route("/User/{Id}", "DELETE")]
    public class DeleteUser : User { }
    #endregion

    #region Generic Documents
    [Route("/User/Finalize", "POST")]
    public class FinalizeUser : User { }

    [Route("/User/Unfinalize", "POST")]
    public class UnfinalizeUser : User { }

    [Route("/User/MakeRevision", "POST")]
    public class MakeUserRevision : User { }

    [Route("/User/Checkout", "POST")]
    public class CheckoutUser : User { }

    [Route("/User/CancelCheckout", "POST")]
    public class CancelCheckoutUser : User { }

    [Route("/User/Checkin", "POST")]
    public class CheckinUser : User { }

    [Route("/User/CreateAndCheckout", "POST")]
    public class CreateAndCheckoutUser : User { }
    #endregion
}
