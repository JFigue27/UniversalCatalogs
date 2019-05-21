using Reusable.CRUD.Entities;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.CRUD.Contract
{
    public abstract class BaseDocument : ITrack
    {
        virtual public bool IsLockedOut { get; set; }

        virtual public string DocumentStatus { get; set; }

        [Reference]
        public User CheckedoutBy { get; set; }
        public long? CheckedoutById { get; set; }

        [Ignore]
        [NotMapped]
        public List<Revision> Revisions { get; set; }

        public string RevisionMessage { get; set; }
    }
}
