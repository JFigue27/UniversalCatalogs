using Reusable.CRUD.Contract;
using System;

namespace Reusable.CRUD.Entities
{
    public class Token : BaseEntity
    {
        public Token()
        {
            CreatedAt = DateTimeOffset.Now;
        }

        public string Value { get; set; }
        public string Subject { get; set; }
        public long SubjectKey { get; set; }
        public DateTimeOffset? DeadDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
