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
    public class AdditionalFieldLogic : LogicWrite<AdditionalField>, ILogicWriteAsync<AdditionalField>
    {
        

        

        protected override AdditionalField OnCreateInstance(AdditionalField entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<AdditionalField> OnGetList(SqlExpression<AdditionalField> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<AdditionalField> OnGetSingle(SqlExpression<AdditionalField> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(AdditionalField entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(AdditionalField entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(AdditionalField entity)
        {
            
            
        }

        protected override IEnumerable<AdditionalField> AdapterOut(params AdditionalField[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
