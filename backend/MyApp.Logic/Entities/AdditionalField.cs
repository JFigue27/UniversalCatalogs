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
    public class AdditionalField : BaseEntity
    {
        public AdditionalField()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public object Value { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
