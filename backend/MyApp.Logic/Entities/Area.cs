using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Logic.Entities
{
    public class Area : BaseEntity
    {
        public Area()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string Value { get; set; }
        
        [Reference]
        public List<Employee> Supervisors { get; set; }

        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
