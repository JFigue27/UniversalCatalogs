using MyApp.Logic.Entities;
using Reusable.Attachments;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS;
using Reusable.CRUD.JsonEntities;
using Reusable.EmailServices;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;



namespace MyApp.Logic
{
    public class EmployeeLogic : LogicWrite<Employee>, ILogicWriteAsync<Employee>
    {
        

        

        protected override Employee OnCreateInstance(Employee entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Employee> OnGetList(SqlExpression<Employee> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Employee> OnGetSingle(SqlExpression<Employee> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Employee entity)
        {
            
            
        }

        protected override IEnumerable<Employee> AdapterOut(params Employee[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
