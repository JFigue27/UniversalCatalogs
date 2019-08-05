using Reusable.Attachments;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.JsonEntities;
using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Logic.Entities
{
    public class User : BaseDocument
    {
        public User()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

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
        
        [NotMapped]
        [Ignore]
        public string Password { get; set; }
        
        [NotMapped]
        [Ignore]
        public string ConfirmPassword { get; set; }
        
        [NotMapped]
        [Ignore]
        public bool ChangePassword { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
