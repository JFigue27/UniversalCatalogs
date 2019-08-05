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
    public class FilterDataLogic : LogicWrite<FilterData>, ILogicWriteAsync<FilterData>
    {
        

        

        protected override FilterData OnCreateInstance(FilterData entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<FilterData> OnGetList(SqlExpression<FilterData> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<FilterData> OnGetSingle(SqlExpression<FilterData> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(FilterData entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(FilterData entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(FilterData entity)
        {
            
            
        }

        protected override IEnumerable<FilterData> AdapterOut(params FilterData[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
