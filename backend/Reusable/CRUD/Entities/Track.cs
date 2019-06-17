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

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string RemovedBy { get; set; }

        public string AssignedTo { get; set; }

        public string AssignedBy { get; set; }
    }
}
