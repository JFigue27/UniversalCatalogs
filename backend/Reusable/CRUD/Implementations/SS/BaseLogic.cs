using Reusable.CRUD.Entities;
using Reusable.Rest;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using System;
using System.Data;

namespace Reusable.CRUD.Implementations.SS
{
    public abstract class BaseLogic : Contract.ILogic
    {
        public LoggedUser LoggedUser { get; set; }
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
            LoggedUser = new LoggedUser(); //Anonymous by default.

            var localUser = Db.Single(Db.From<User>()
                    .Where(u => u.UserName == Auth.UserName || u.Email == Auth.Email));

            if (localUser != null)
            {
                LoggedUser = new LoggedUser
                {
                    Email = Auth.Email,
                    IdentityProvider = Auth.AuthProvider,
                    UserName = Auth.UserName,
                    Value = Auth.DisplayName,
                    LocalUser = localUser,
                    UserID = localUser.Id
                };
            }
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
    }
}
