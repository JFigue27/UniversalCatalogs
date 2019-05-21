using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.CRUD.Entities
{
    public class Track : BaseEntity
    {
        [NotMapped]
        public string Discriminator { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public DateTimeOffset? RemovedAt { get; set; }

        public DateTimeOffset? UsedAt { get; set; }

        [Reference]
        public virtual User CreatedBy { get; set; }
        public long? CreatedById { get; set; }

        [Reference]
        public virtual User UpdatedBy { get; set; }
        public long? UpdatedById { get; set; }

        [Reference]
        public virtual User RemovedBy { get; set; }
        public long? RemovedById { get; set; }

        [Reference]
        public virtual User AssignedTo { get; set; }
        public long? AssignedToId { get; set; }

        [Reference]
        public virtual User AssignedBy { get; set; }
        public long? AssignedById { get; set; }

    }
}
