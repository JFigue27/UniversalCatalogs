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
    public class CatalogTypeLogic : LogicWrite<CatalogType>, ILogicWriteAsync<CatalogType>
    {
        

        

        protected override CatalogType OnCreateInstance(CatalogType entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<CatalogType> OnGetList(SqlExpression<CatalogType> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<CatalogType> OnGetSingle(SqlExpression<CatalogType> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(CatalogType entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) throw new KnownError("Invalid Name.");

        if (mode == OPERATION_MODE.UPDATE)
        {
            var original = GetById(entity.Id);
            if (original == null) throw new KnownError("Error. Entity no longer exists.");

            if (original.Name != entity.Name)
            {
                Db.Update<Catalog>(new { CatalogType = entity.Name }, e => e.CatalogType == original.Name);
                Db.Update<CatalogType>(new { ParentType = entity.Name }, e => e.ParentType == original.Name);
                Db.Update<Catalog>(new { Parent = entity.Name }, e => e.Parent == original.Name);
            }
        }

            
        }

        protected override void OnAfterSaving(CatalogType entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            Cache.FlushAll();
            
        }

        protected override void OnBeforeRemoving(CatalogType entity)
        {
            
            
        }

        protected override IEnumerable<CatalogType> AdapterOut(params CatalogType[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
