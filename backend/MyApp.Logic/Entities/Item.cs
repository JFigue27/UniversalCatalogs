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
    public class Item : BaseEntity
    {
        public Item()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
