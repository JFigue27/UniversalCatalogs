using Reusable.CRUD.Contract;
using Reusable.EmailServices;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Reusable.CRUD.Implementations.SS
{
    public abstract class BaseLogic : ILogic
    {
        public IRequest Request { get; set; }
        public IDbConnection Db { get; set; }
        public ICacheClient Cache { get; set; }
        public IAuthSession Auth { get; set; }
        public IEmailService EmailService { get; set; }

        public virtual void Init(IDbConnection db, IAuthSession auth, IRequest request)
        {
            Db = db;
            Auth = auth;
            Request = request;
        }

        public bool HasRoles(params string[] roles)
        {
            if (Auth != null && Auth.Roles != null)
            {
                foreach (var role in roles)
                    if (!Auth.Roles.Contains(role))
                        return false;
            }
            else
                return false;

            return true;
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
            public int page { get; set; }
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
                "filterUser",
                null
            }.Contains(param))
                return false;

            return true;
        }
    }
}
