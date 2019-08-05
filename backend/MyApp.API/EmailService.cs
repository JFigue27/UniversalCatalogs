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
    public class EmailService : BaseService<EmailLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllEmails request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetEmailById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetEmailWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedEmails request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateEmailInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Email>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Generic Document
        virtual public object Post(FinalizeEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Finalize(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(UnfinalizeEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Unfinalize(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(MakeEmailRevision request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.MakeRevision(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CheckoutEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Checkout(entity.Id);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CancelCheckoutEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.CancelCheckout(entity.Id);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CheckinEmail request)
        {
            var entity = request.ConvertTo<Email>();
            return InTransaction(db => {
                Logic.Checkin(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        virtual public object Post(CreateAndCheckoutEmail request)
        {
            var entity = request.ConvertTo<Email>();
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
    [Route("/Email", "GET")]
    public class GetAllEmails : GetAll<Email> { }

    [Route("/Email/{Id}", "GET")]
    public class GetEmailById : GetSingleById<Email> { }

    [Route("/Email/GetSingleWhere", "GET")]
    [Route("/Email/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetEmailWhere : GetSingleWhere<Email> { }

    [Route("/Email/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedEmails : GetPaged<Email> { }
    #endregion

    #region Generic Write
    [Route("/Email/CreateInstance", "POST")]
    public class CreateEmailInstance : Email { }

    [Route("/Email", "POST")]
    public class InsertEmail : Email { }

    [Route("/Email", "PUT")]
    public class UpdateEmail : Email { }

    [Route("/Email", "DELETE")]
    [Route("/Email/{Id}", "DELETE")]
    public class DeleteEmail : Email { }
    #endregion

    #region Generic Documents
    [Route("/Email/Finalize", "POST")]
    public class FinalizeEmail : Email { }

    [Route("/Email/Unfinalize", "POST")]
    public class UnfinalizeEmail : Email { }

    [Route("/Email/MakeRevision", "POST")]
    public class MakeEmailRevision : Email { }

    [Route("/Email/Checkout", "POST")]
    public class CheckoutEmail : Email { }

    [Route("/Email/CancelCheckout", "POST")]
    public class CancelCheckoutEmail : Email { }

    [Route("/Email/Checkin", "POST")]
    public class CheckinEmail : Email { }

    [Route("/Email/CreateAndCheckout", "POST")]
    public class CreateAndCheckoutEmail : Email { }
    #endregion
}
