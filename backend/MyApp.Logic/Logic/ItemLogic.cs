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
    public class ItemLogic : LogicWrite<Item>, ILogicWriteAsync<Item>
    {
        

        

        protected override Item OnCreateInstance(Item entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Item> OnGetList(SqlExpression<Item> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Item> OnGetSingle(SqlExpression<Item> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Item entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Item entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Item entity)
        {
            
            
        }

        protected override IEnumerable<Item> AdapterOut(params Item[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
