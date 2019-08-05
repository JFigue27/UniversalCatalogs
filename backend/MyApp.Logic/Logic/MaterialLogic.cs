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
    public class MaterialLogic : LogicWrite<Material>, ILogicWriteAsync<Material>
    {
        

        

        protected override Material OnCreateInstance(Material entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Material> OnGetList(SqlExpression<Material> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Material> OnGetSingle(SqlExpression<Material> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Material entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Material entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Material entity)
        {
            
            
        }

        protected override IEnumerable<Material> AdapterOut(params Material[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
