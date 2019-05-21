using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class RoleService : Service
    {
        public IRoleLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllRoles request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetRoleById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetRoleWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedRoles request)
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
        public object Post(CreateRoleInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Role>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertRole request)
        {
            var entity = request.ConvertTo<Role>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateRole request)
        {
            var entity = request.ConvertTo<Role>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteRole request)
        {
            var entity = request.ConvertTo<Role>();
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
    [Route("/Role", "GET")]
    public class GetAllRoles : GetAll<Role> { }

    [Route("/Role/{Id}", "GET")]
    public class GetRoleById : GetSingleById<Role> { }

    [Route("/Role/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetRoleWhere : GetSingleWhere<Role> { }

    [Route("/Role/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedRoles : GetPaged<Role> { }
    #endregion


    #region Generic Write
    [Route("/Role/CreateInstance", "GET")]
    public class CreateRoleInstance : Role { }

    [Route("/Role", "POST")]
    public class InsertRole : Role { }

    [Route("/Role", "PUT")]
    public class UpdateRole : Role { }

    [Route("/Role/{Id}", "DELETE")]
    public class DeleteRole : Role { }
    #endregion

}
