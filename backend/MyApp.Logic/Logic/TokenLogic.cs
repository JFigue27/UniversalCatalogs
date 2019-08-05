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
    public class TokenLogic : LogicWrite<Token>, ILogicWriteAsync<Token>
    {
        

        

        protected override Token OnCreateInstance(Token entity)
        {
            
            

            return entity;
        }

        protected override SqlExpression<Token> OnGetList(SqlExpression<Token> query)
        {
            
            

            return query;
        }

        protected override SqlExpression<Token> OnGetSingle(SqlExpression<Token> query)
        {
            
            

            return query;
        }

        protected override void OnBeforeSaving(Token entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            entity.CreatedAt = DateTimeOffset.Now;
            
        }

        protected override void OnAfterSaving(Token entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            
        }

        protected override void OnBeforeRemoving(Token entity)
        {
            
            
        }

        protected override IEnumerable<Token> AdapterOut(params Token[] entities)
        {
            

            foreach (var item in entities)
            {
                
            }

            return entities;
        }

        
        
    }
}
