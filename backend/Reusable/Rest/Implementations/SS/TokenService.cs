using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class TokenService : Service
    {
        public ITokenLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllTokens request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetTokenById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetTokenWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedTokens request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetPagedAsync(
                request.Limit,
                request.Page,
                request.FilterGeneral,
                null,
                null);
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateTokenInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Token>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertToken request)
        {
            var entity = request.ConvertTo<Token>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateToken request)
        {
            var entity = request.ConvertTo<Token>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteToken request)
        {
            var entity = request.ConvertTo<Token>();
            Logic.Remove(entity);
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        #endregion

    }

    #region Specific
    #endregion



    #region Generic Read Only
    [Route("/Token", "GET")]
    public class GetAllTokens : GetAll<Token> { }

    [Route("/Token/{Id}", "GET")]
    public class GetTokenById : GetSingleById<Token> { }

    [Route("/Token/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetTokenWhere : GetSingleWhere<Token> { }

    [Route("/Token/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedTokens : GetPaged<Token> { }
    #endregion


    #region Generic Write
    [Route("/Token/CreateInstance", "POST")]
    public class CreateTokenInstance : Token { }

    [Route("/Token", "POST")]
    public class InsertToken : Token { }

    [Route("/Token", "PUT")]
    public class UpdateToken : Token { }

    [Route("/Token/{Id}", "DELETE")]
    public class DeleteToken : Token { }
    #endregion

}
