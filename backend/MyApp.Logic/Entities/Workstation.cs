using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Logic.Entities
{
    public class Workstation : BaseEntity
    {
        public Workstation()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string Value { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}