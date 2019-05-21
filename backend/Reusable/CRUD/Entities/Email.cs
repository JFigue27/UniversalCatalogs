using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Reusable.Attachments;
using Reusable.CRUD.Contract;
using Reusable.CRUD.JsonEntities;
using ServiceStack.DataAnnotations;

namespace Reusable.CRUD.Entities
{
    public class Email : BaseDocument, IAttachment
    {
        public Email()
        {
            To = new List<Contact>();
            Cc = new List<Contact>();
            Bcc = new List<Contact>();
        }

        public List<Contact> To { get; set; }
        public List<Contact> Cc { get; set; }
        public List<Contact> Bcc { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public string AdditionalNote { get; set; }

        public string AttachmentsFolder { get; set; }
        public List<Attachment> Attachments { get; set; }

        [Ignore]
        [NotMapped]
        public bool IsResent { get; set; }
    }
}
