using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;

namespace Reusable.CRUD.Entities
{
    public class ApprovalApprover : BaseEntity
    {
        [Reference]
        public Approval Approval { get; set; }
        public long ApprovalId { get; set; }

        [Reference]
        public User Approver { get; set; }
        public long ApproverId { get; set; }
    }
}
