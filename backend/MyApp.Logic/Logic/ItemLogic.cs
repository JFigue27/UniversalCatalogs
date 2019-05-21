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
    public interface IItemLogic : ILogicWrite<Item>, ILogicWriteAsync<Item>
    {
        ///start:slot:interface<<<///end:slot:interface<<<
    }

    public class ItemLogic : LogicWrite<Item>, ILogicWriteAsync<Item>, IItemLogic
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Item OnCreateInstance(Item entity)
        {
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Item> OnGetList(SqlExpression<Item> query)
        {
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Item> OnGetSingle(SqlExpression<Item> query)
        {
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Item entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Item entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Item entity)
        {
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Item> AdapterOut(params Item[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            return entities;
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
