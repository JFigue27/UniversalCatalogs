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
    public class Email : BaseDocument
    {
        public Email()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public List<Contact> To { get; set; }
        public List<Contact> Cc { get; set; }
        public List<Contact> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AdditionalNote { get; set; }
        public string AttachmentsFolder { get; set; }
        public List<Attachment> Attachments { get; set; }
        
        [NotMapped]
        [Ignore]
        public bool IsResent { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
