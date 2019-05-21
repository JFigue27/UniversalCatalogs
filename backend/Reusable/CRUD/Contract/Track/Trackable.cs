using Reusable.CRUD.Entities;
using ServiceStack.DataAnnotations;
using System;

namespace Reusable.CRUD.Contract
{
    public abstract class ITrack : BaseEntity
    {
        public ITrack()
        {
            CreatedAt = DateTimeOffset.Now;
        }

        virtual public DateTimeOffset CreatedAt { get; set; }

        virtual public bool IsDeleted { get; set; }

        [Reference]
        public Track Track { get; set; }
        public long? TrackId { get; set; }
    }
}
