using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace Reusable.CRUD.Entities
{
    public class Application : BaseDocument
    {
        public string Name { get; set; }

        [Reference]
        public List<Role> Roles { get; set; }
    }
}
