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




namespace MyApp.Logic
{
    public class CatalogTypeFieldLogic : WriteLogic<CatalogTypeField>, ILogicWriteAsync<CatalogTypeField>
    {
        
        

        

        protected override CatalogTypeField OnCreateInstance(CatalogTypeField entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<CatalogTypeField> OnGetList(SqlExpression<CatalogTypeField> query)
        {
            
            

            return base.OnGetList(query);
        }

        protected override SqlExpression<CatalogTypeField> OnGetSingle(SqlExpression<CatalogTypeField> query)
        {
            
            

            return base.OnGetSingle(query);
        }

        protected override void OnBeforeSaving(CatalogTypeField entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(CatalogTypeField entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(CatalogTypeField entity)
        {
            
            
        }

        protected override List<CatalogTypeField> AdapterOut(params CatalogTypeField[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities.ToList();
        }

        protected override void OnFinalize(CatalogTypeField entity)
        {
            
        }

        protected override void OnUnfinalize(CatalogTypeField entity)
        {
            
        }

        
        
    }
}
