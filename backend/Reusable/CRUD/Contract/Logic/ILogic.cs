using Reusable.Rest;
using ServiceStack.Auth;
using ServiceStack.Web;
using System.Data;

namespace Reusable.CRUD.Contract
{
    public interface ILogic
    {
        LoggedUser LoggedUser { get; set; }
        IRequest Request { get; set; }
        IDbConnection Db { get; set; }
        void SetDb(IDbConnection Db);
        void SetAuth(IAuthSession Auth);
    }
}
