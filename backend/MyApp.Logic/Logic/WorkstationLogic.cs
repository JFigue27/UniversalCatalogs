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
    public interface IWorkstationLogic : ILogicWrite<Workstation>, ILogicWriteAsync<Workstation>
    {
        ///start:slot:interface<<<///end:slot:interface<<<
    }

    public class WorkstationLogic : LogicWrite<Workstation>, ILogicWriteAsync<Workstation>, IWorkstationLogic
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Workstation OnCreateInstance(Workstation entity)
        {
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Workstation> OnGetList(SqlExpression<Workstation> query)
        {
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Workstation> OnGetSingle(SqlExpression<Workstation> query)
        {
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Workstation entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Workstation entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Workstation entity)
        {
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Workstation> AdapterOut(params Workstation[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            return entities;
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
