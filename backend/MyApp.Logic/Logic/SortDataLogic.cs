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
    public class SortDataLogic : LogicWrite<SortData>, ILogicWriteAsync<SortData>
    {
        

        

        protected override SortData OnCreateInstance(SortData entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<SortData> OnGetList(SqlExpression<SortData> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<SortData> OnGetSingle(SqlExpression<SortData> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(SortData entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnAfterSaving(SortData entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(SortData entity)
        {
            
            
        }

        protected override IEnumerable<SortData> AdapterOut(params SortData[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
