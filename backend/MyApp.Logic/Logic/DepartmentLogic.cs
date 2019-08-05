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
    public class DepartmentLogic : LogicWrite<Department>, ILogicWriteAsync<Department>
    {
        

        

        protected override Department OnCreateInstance(Department entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Department> OnGetList(SqlExpression<Department> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Department> OnGetSingle(SqlExpression<Department> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Department entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(Department entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Department entity)
        {
            
            
        }

        protected override IEnumerable<Department> AdapterOut(params Department[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
