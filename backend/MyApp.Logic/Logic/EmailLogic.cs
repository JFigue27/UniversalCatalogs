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

///start:slot:imports<<<///end:slot:imports<<<


namespace MyApp.Logic
{
    public class EmailLogic : DocumentLogic<Email>, IDocumentLogicAsync<Email>
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Email OnCreateInstance(Email entity)
        {
            
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Email> OnGetList(SqlExpression<Email> query)
        {
            
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return base.OnGetList(query);
        }

        protected override SqlExpression<Email> OnGetSingle(SqlExpression<Email> query)
        {
            
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return base.OnGetSingle(query);
        }

        protected override void OnBeforeSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Email entity)
        {
            
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Email> AdapterOut(params Email[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        protected override void OnFinalize(Email entity)
        {
            ///start:slot:finalize<<<///end:slot:finalize<<<
        }

        protected override void OnUnfinalize(Email entity)
        {
            ///start:slot:unfinalize<<<///end:slot:unfinalize<<<
        }

        
        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
