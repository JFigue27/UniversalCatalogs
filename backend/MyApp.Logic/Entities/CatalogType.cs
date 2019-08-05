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
    public class CatalogType : BaseEntity
    {
        public CatalogType()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string Name { get; set; }
        public string ParentType { get; set; }
        public string Fields { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
