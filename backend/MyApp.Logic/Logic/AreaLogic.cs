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
    public class AreaLogic : LogicWrite<Area>, ILogicWriteAsync<Area>
    {
        

        

        protected override Area OnCreateInstance(Area entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Area> OnGetList(SqlExpression<Area> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Area> OnGetSingle(SqlExpression<Area> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Area entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Area entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Area entity)
        {
            
            
        }

        protected override IEnumerable<Area> AdapterOut(params Area[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
