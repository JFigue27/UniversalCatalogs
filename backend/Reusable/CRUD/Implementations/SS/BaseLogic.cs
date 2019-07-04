using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.Rest;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Reusable.CRUD.Implementations.SS
{
    public abstract class BaseLogic : Contract.ILogic
    {
        //public LoggedUser LoggedUser { get; set; }
        public IRequest Request { get; set; }
        public IDbConnection Db { get; set; }
        public ICacheClient Cache { get; set; }
        public IAuthSession Auth { get; set; }

        public virtual void SetDb(IDbConnection Db)
        {
            this.Db = Db;
        }

        public virtual void SetAuth(IAuthSession Auth)
        {
            this.Auth = Auth;
            //LoggedUser = new LoggedUser(); //Anonymous by default.

            //var localUser = Db.Single(Db.From<User>()
            //        .Where(u => u.UserName == Auth.UserName || u.Email == Auth.Email));

            //if (localUser != null)
            //{
            //    LoggedUser = new LoggedUser
            //    {
            //        Email = Auth.Email,
            //        IdentityProvider = Auth.AuthProvider,
            //        UserName = Auth.UserName,
            //        Value = Auth.DisplayName,
            //        LocalUser = localUser,
            //        UserID = localUser.Id
            //    };
            //}
        }

        static public object TryCatch(Action operation)
        {
            try
            {
                operation();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        protected class FilterResponse
        {
            public List<List<BaseEntity>> Dropdowns { get; set; }
            public int total_items { get; set; }
            public int total_filtered_items { get; set; }
        }

        protected bool IsValidJSValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null" || value == "undefined")
            {
                return false;
            }

            return true;
        }

        public Exception GetOriginalException(Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return GetOriginalException(ex.InnerException);
        }

        protected bool IsValidParam(string param)
        {
            //reserved and invalid params:
            if (new string[] {
                "limit",
                "perPage",
                "page",
                "filterGeneral",
                "itemsCount",
                "noCache",
                "totalItems",
                "parentKey",
                "parentField",
                "filterUser"
            }.Contains(param))
                return false;

            return true;
        }
    }
}
