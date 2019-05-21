using Reusable.CRUD.Entities;
using Reusable.Rest;

namespace Reusable.CRUD.Contract
{
    public interface IApprovalLogic : IDocumentLogic<Approval>, IDocumentLogicAsync<Approval>
    {
        void SetStatus(long ApprovalKey, string Message, string Status);
    }
}