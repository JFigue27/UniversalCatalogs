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
    public interface IShiftLogic : ILogicWrite<Shift>, ILogicWriteAsync<Shift>
    {
        ///start:slot:interface<<<///end:slot:interface<<<
    }

    public class ShiftLogic : LogicWrite<Shift>, ILogicWriteAsync<Shift>, IShiftLogic
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Shift OnCreateInstance(Shift entity)
        {
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Shift> OnGetList(SqlExpression<Shift> query)
        {
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Shift> OnGetSingle(SqlExpression<Shift> query)
        {
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Shift entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Shift entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Shift entity)
        {
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Shift> AdapterOut(params Shift[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            return entities;
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
