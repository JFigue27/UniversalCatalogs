using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace Reusable.CRUD.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        [Reference]
        public Application Application { get; set; }
        public long ApplicationId { get; set; }

        [Reference]
        public ICollection<User> Users { get; set; }
    }
}
