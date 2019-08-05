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
    public class EmailLogic : DocumentLogic<Email>, IDocumentLogicAsync<Email>
    {
        

        

        protected override Email OnCreateInstance(Email entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Email> OnGetList(SqlExpression<Email> query)
        {
            
            

            return base.OnGetList(query);
        }

        protected override SqlExpression<Email> OnGetSingle(SqlExpression<Email> query)
        {
            
            

            return base.OnGetSingle(query);
        }

        protected override void OnBeforeSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Email entity)
        {
            
            
        }

        protected override IEnumerable<Email> AdapterOut(params Email[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        protected override void OnFinalize(Email entity)
        {
            
        }

        protected override void OnUnfinalize(Email entity)
        {
            
        }

        
        
    }
}
