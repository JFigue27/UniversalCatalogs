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
    public class CatalogLogic : LogicWrite<Catalog>, ILogicWriteAsync<Catalog>
    {
        

        

        protected override Catalog OnCreateInstance(Catalog entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Catalog> OnGetList(SqlExpression<Catalog> query)
        {
            var name = Request.QueryString["name"];
        if (IsValidJSValue(name))
            query.Where(e => e.CatalogType == name);

            

            return query;
        }

        protected override SqlExpression<Catalog> OnGetSingle(SqlExpression<Catalog> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Catalog entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (string.IsNullOrWhiteSpace(entity.CatalogType))
            throw new KnownError("Error. Catalog Type is a required field.");

            
        }

        protected override void OnAfterSaving(Catalog entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Catalog entity)
        {
            
            
        }

        protected override IEnumerable<Catalog> AdapterOut(params Catalog[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
