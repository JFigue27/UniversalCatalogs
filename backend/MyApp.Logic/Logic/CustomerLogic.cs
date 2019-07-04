using MyApp.Logic.Entities;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyApp.Logic
{
    public interface ICustomerLogic : ILogicWrite<Customer>, ILogicWriteAsync<Customer>
    {
        ///start:slot:interface<<<///end:slot:interface<<<
    }

    public class CustomerLogic : LogicWrite<Customer>, ILogicWriteAsync<Customer>, ICustomerLogic
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Customer OnCreateInstance(Customer entity)
        {
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Customer> OnGetList(SqlExpression<Customer> query)
        {
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Customer> OnGetSingle(SqlExpression<Customer> query)
        {
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Customer entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Customer entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Customer entity)
        {
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Customer> AdapterOut(params Customer[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            return entities;
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
