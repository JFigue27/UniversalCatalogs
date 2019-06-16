using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.CRUD.Entities
{
    public partial class User : BaseDocument
    {
        public string Value { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string AuthorizatorPassword { get; set; }

        public string Email { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public byte[] Identicon { get; set; }

        public string Identicon64 { get; set; }

        public string EmailPassword { get; set; }

        public string EmailServer { get; set; }

        public string EmailPort { get; set; }

        [Reference]
        public Department Department { get; set; }
        public long? DepartmentId { get; set; }


        [Ignore]
        [NotMapped]
        public string Password { get; set; }

        [Ignore]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Ignore]
        [NotMapped]
        public bool ChangePassword { get; set; }
    }
}
