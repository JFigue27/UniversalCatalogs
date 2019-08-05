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
    public class WorkstationLogic : LogicWrite<Workstation>, ILogicWriteAsync<Workstation>
    {
        

        

        protected override Workstation OnCreateInstance(Workstation entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Workstation> OnGetList(SqlExpression<Workstation> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Workstation> OnGetSingle(SqlExpression<Workstation> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Workstation entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Workstation entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Workstation entity)
        {
            
            
        }

        protected override IEnumerable<Workstation> AdapterOut(params Workstation[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
