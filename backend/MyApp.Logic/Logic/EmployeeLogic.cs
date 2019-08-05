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
    public class EmployeeLogic : LogicWrite<Employee>, ILogicWriteAsync<Employee>
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Employee OnCreateInstance(Employee entity)
        {
            
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Employee> OnGetList(SqlExpression<Employee> query)
        {
            
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Employee> OnGetSingle(SqlExpression<Employee> query)
        {
            
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Employee entity)
        {
            
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Employee> AdapterOut(params Employee[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
