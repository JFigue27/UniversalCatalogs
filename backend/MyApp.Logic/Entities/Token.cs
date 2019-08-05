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
    public class Token : BaseEntity
    {
        public Token()
        {
            CreatedAt = DateTimeOffset.Now;
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string Value { get; set; }
        public string ForeignType { get; set; }
        public long ForeignKey { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
