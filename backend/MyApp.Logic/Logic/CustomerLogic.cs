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
    public class CustomerLogic : LogicWrite<Customer>, ILogicWriteAsync<Customer>
    {
        

        

        protected override Customer OnCreateInstance(Customer entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Customer> OnGetList(SqlExpression<Customer> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Customer> OnGetSingle(SqlExpression<Customer> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Customer entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Customer entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Customer entity)
        {
            
            
        }

        protected override IEnumerable<Customer> AdapterOut(params Customer[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
