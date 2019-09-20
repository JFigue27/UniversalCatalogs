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
    public class Employee : BaseEntity
    {
        public Employee()
        {
            
            ///start:slot:ctor<<<///end:slot:ctor<<<
        }

        public string Value { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string CURP { get; set; }
        public string PersonalNumber { get; set; }
        public string TimeIdNumber { get; set; }
        public string STPSPosition { get; set; }
        public int Area { get; set; }
        public int Shift { get; set; }
        public int JobPosition { get; set; }
        public int SupervisedBy { get; set; }

        
        ///start:slot:properties<<<///end:slot:properties<<<
    }
}
