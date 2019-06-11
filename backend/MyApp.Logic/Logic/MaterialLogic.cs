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
    public interface IMaterialLogic : ILogicWrite<Material>, ILogicWriteAsync<Material>
    {
        ///start:slot:interface<<<///end:slot:interface<<<
    }

    public class MaterialLogic : LogicWrite<Material>, ILogicWriteAsync<Material>, IMaterialLogic
    {
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Material OnCreateInstance(Material entity)
        {
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Material> OnGetList(SqlExpression<Material> query)
        {
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Material> OnGetSingle(SqlExpression<Material> query)
        {
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Material entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Material entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Material entity)
        {
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override IEnumerable<Material> AdapterOut(params Material[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            return entities;
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
