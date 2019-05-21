using System;
using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;

namespace Reusable.CRUD.Entities
{
    public class Task : BaseEntity
    {
        public Task()
        {
            DateCreatedAt = DateTimeOffset.Now;
        }

        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }


        [Reference]
        public User UserCreatedBy { get; set; }
        public long? UserCreatedById { get; set; }

        [Reference]
        public User UserAssignedTo { get; set; }
        public long? UserAssignedToId { get; set; }

        [Reference]
        public User UserCompletedBy { get; set; }
        public long? UserCompletedById { get; set; }

        public DateTimeOffset DateCreatedAt { get; set; }
        public DateTimeOffset? DateDue { get; set; }
        public DateTimeOffset? DateClosed { get; set; }

        public long? ForeignKey { get; set; }
        public string ForeignType { get; set; }
        public long? ForeignURLKey { get; set; }
        public string ForeignURLType { get; set; }

        public bool? IsCancelled { get; set; }

        public enum TaskStatus
        {
            PENDING,
            IN_PROGRESS,
            COMPLETED,
            CANCELLED,
            ON_HOLD
        }

        public enum TaskPriority
        {
            LOW,
            MEDIUM,
            HIGH,
            URGENT
        }
    }
}
