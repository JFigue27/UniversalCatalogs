using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class UserService : Service
    {
        public IUserLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllUsers request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetUserById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetUserWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedUsers request)
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
        public object Post(CreateUserInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<User>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertUser request)
        {
            var entity = request.ConvertTo<User>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateUser request)
        {
            var entity = request.ConvertTo<User>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteUser request)
        {
            var entity = request.ConvertTo<User>();
            Logic.Remove(entity);
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        public async Task<object> Get(GetByUserName request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByUserNameAsync(request.UserName);
        }
        public async Task<object> Get(GetByRole request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByRoleAsync(request.Role);
        }
        #endregion
    }

    #region Specific
    [Route("/User", "GET")]
    [Route("/User/{UserName}", "GET")]
    public class GetByUserName
    {
        public string UserName { get; set; }
    }

    [Route("/User", "GET")]
    [Route("/User/{Role}", "GET")]
    public class GetByRole
    {
        public string Role { get; set; }
    }
    #endregion


    #region Generic Read Only
    [Route("/User", "GET")]
    public class GetAllUsers : GetAll<User> { }

    [Route("/User/{Id}", "GET")]
    public class GetUserById : GetSingleById<User> { }

    [Route("/User/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetUserWhere : GetSingleWhere<User> { }

    [Route("/User/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedUsers : GetPaged<User> { }
    #endregion


    #region Generic Write
    [Route("/User/CreateInstance", "POST")]
    public class CreateUserInstance : User { }

    [Route("/User", "POST")]
    public class InsertUser : User { }

    [Route("/User", "PUT")]
    public class UpdateUser : User { }

    [Route("/User/{Id}", "DELETE")]
    public class DeleteUser : User { }
    #endregion

}
