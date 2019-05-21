using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class ApprovalService : Service
    {
        public IApprovalLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllApprovals request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetApprovalById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetApprovalWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedApprovals request)
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
        public object Post(CreateApprovalInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Approval>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.Remove(entity);
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Generic Document
        virtual public object Post(FinalizeApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.Finalize(entity);
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(UnfinalizeApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.Unfinalize(entity);
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(MakeApprovalRevision request)
        {
            var entity = request.ConvertTo<Approval>();
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(CheckoutApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.Checkout(entity.Id);
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(CancelCheckoutApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.CancelCheckout(entity.Id);
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(CheckinApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.Checkin(entity);
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        virtual public object Post(CreateAndCheckoutApproval request)
        {
            var entity = request.ConvertTo<Approval>();
            Logic.CreateAndCheckout(entity);
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        #endregion

        #region Endpoints - Specific
        public object Post(ApproveApproval request)
        {
            Logic.SetStatus(request.ApprovalId, request.ResponseDescription, "Approved");
            return new CommonResponse(Logic.GetById(request.ApprovalId));
        }

        public object Post(RejectApproval request)
        {
            Logic.SetStatus(request.ApprovalId, request.ResponseDescription, "Rejected");
            return new CommonResponse(Logic.GetById(request.ApprovalId));
        }


        #endregion

    }

    #region Specific
    [Route("/Approval/Approve/{ApprovalId}", "POST")]
    [Route("/Approval/Approve/{ApprovalId}/{ResponseDescription}", "POST")]
    public class ApproveApproval
    {
        public long ApprovalId { get; set; }
        public string ResponseDescription { get; set; }
    }

    [Route("/Approval/Reject/{ApprovalId}", "POST")]
    [Route("/Approval/Reject/{ApprovalId}/{ResponseDescription}", "POST")]
    public class RejectApproval
    {
        public long ApprovalId { get; set; }
        public string ResponseDescription { get; set; }
    }
    #endregion



    #region Generic Read Only
    [Route("/Approval", "GET")]
    public class GetAllApprovals : GetAll<Approval> { }

    [Route("/Approval/{Id}", "GET")]
    public class GetApprovalById : GetSingleById<Approval> { }

    [Route("/Approval/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetApprovalWhere : GetSingleWhere<Approval> { }

    [Route("/Approval/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedApprovals : GetPaged<Approval> { }
    #endregion


    #region Generic Write
    [Route("/Approval/CreateInstance", "POST")]
    public class CreateApprovalInstance : Approval { }

    [Route("/Approval", "POST")]
    public class InsertApproval : Approval { }

    [Route("/Approval", "PUT")]
    public class UpdateApproval : Approval { }

    [Route("/Approval/{Id}", "DELETE")]
    public class DeleteApproval : Approval { }
    #endregion

    #region Generic Documents
    [Route("/Approval/Finalize", "POST")]
    public class FinalizeApproval : Approval { }

    [Route("/Approval/Unfinalize", "POST")]
    public class UnfinalizeApproval : Approval { }

    [Route("/Approval/MakeRevision", "POST")]
    public class MakeApprovalRevision : Approval { }

    [Route("/Approval/Checkout", "POST")]
    public class CheckoutApproval : Approval { }

    [Route("/Approval/CancelCheckout", "POST")]
    public class CancelCheckoutApproval : Approval { }

    [Route("/Approval/Checkin", "POST")]
    public class CheckinApproval : Approval { }

    [Route("/Approval/CreateAndCheckout", "POST")]
    public class CreateAndCheckoutApproval : Approval { }
    #endregion

}
