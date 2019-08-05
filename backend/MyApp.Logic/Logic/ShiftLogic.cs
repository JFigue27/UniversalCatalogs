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
    public class ShiftLogic : LogicWrite<Shift>, ILogicWriteAsync<Shift>
    {
        

        

        protected override Shift OnCreateInstance(Shift entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Shift> OnGetList(SqlExpression<Shift> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Shift> OnGetSingle(SqlExpression<Shift> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Shift entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Shift entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Shift entity)
        {
            
            
        }

        protected override IEnumerable<Shift> AdapterOut(params Shift[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
