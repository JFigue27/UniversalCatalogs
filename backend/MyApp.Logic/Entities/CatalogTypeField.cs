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
    public class CatalogTypeField : BaseEntity
    {
        public CatalogTypeField()
        {
            
        }

        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string Relationship { get; set; }
        public string Foreign { get; set; }
        
        [Reference]
        public CatalogType CatalogType { get; set; }
        public long CatalogTypeId { get; set; }

        
    }
}
