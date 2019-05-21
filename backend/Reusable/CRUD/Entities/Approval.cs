using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.CRUD.Entities
{
    public class Approval : BaseDocument
    {
        public DateTimeOffset DateRequested { get; set; }
        public string RequestDescription { get; set; }

        [Reference]
        public List<ApprovalApprover> Approvers { get; set; } /* Must be users to login the system and approve,
                                                          this is different from Notify To, where can be external email addresses. */

        public string Title { get; set; }
        public string Type { get; set; }
        public string Hyperlink { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        public long PrimaryForeignKey { get; set; }
        public string PrimaryForeignType { get; set; }

        public long? SecondaryForeignKey { get; set; }
        public string SecondaryForeignType { get; set; }

        public long? TargetRevisionKey { get; set; }


        [Ignore]
        [NotMapped]
        public string Status { get; set; }

        [Ignore]
        [NotMapped]
        public string ResponseDescription { get; set; }
    }
}
