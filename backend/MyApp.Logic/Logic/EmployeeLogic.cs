using MyApp.Logic.Entities;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyApp.Logic
{
    public interface IEmployeeLogic : ILogicWrite<Employee>, ILogicWriteAsync<Employee>
    {
        
    }

    public class EmployeeLogic : LogicWrite<Employee>, ILogicWriteAsync<Employee>, IEmployeeLogic
    {
        

        

        protected override Employee OnCreateInstance(Employee entity)
        {
            

            return entity;
        }

        protected override SqlExpression<Employee> OnGetList(SqlExpression<Employee> query)
        {
            

            return query;
        }

        protected override SqlExpression<Employee> OnGetSingle(SqlExpression<Employee> query)
        {
            

            return query;
        }

        protected override void OnBeforeSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
        }

        protected override void OnAfterSaving(Employee entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
        }

        protected override void OnBeforeRemoving(Employee entity)
        {
            
        }

        protected override IEnumerable<Employee> AdapterOut(params Employee[] entities)
        {
            

            return entities;
        }

        
    }
}
