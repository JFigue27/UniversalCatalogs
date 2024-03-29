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
using ServiceStack.Text;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

///start:slot:imports<<<///end:slot:imports<<<


namespace MyApp.Logic
{
    public class CatalogTypeLogic : WriteLogic<CatalogType>, ILogicWriteAsync<CatalogType>
    {
        
        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override CatalogType OnCreateInstance(CatalogType entity)
        {
            
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<CatalogType> OnGetList(SqlExpression<CatalogType> query)
        {
            
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return base.OnGetList(query);
        }

        protected override SqlExpression<CatalogType> OnGetSingle(SqlExpression<CatalogType> query)
        {
            
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return base.OnGetSingle(query);
        }

        protected override void OnBeforeSaving(CatalogType entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(CatalogType entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(CatalogType entity)
        {
            
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override List<CatalogType> AdapterOut(params CatalogType[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            foreach (var item in entities)
            {
                
            }

            return entities.ToList();
        }

        protected override void OnFinalize(CatalogType entity)
        {
            ///start:slot:finalize<<<///end:slot:finalize<<<
        }

        protected override void OnUnfinalize(CatalogType entity)
        {
            ///start:slot:unfinalize<<<///end:slot:unfinalize<<<
        }

        
        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
