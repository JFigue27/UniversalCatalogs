using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class DepartmentService : Service
    {
        public IDepartmentLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllDepartments request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetDepartmentById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetDepartmentWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedDepartments request)
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
        public object Post(CreateDepartmentInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Department>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertDepartment request)
        {
            var entity = request.ConvertTo<Department>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateDepartment request)
        {
            var entity = request.ConvertTo<Department>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteDepartment request)
        {
            var entity = request.ConvertTo<Department>();
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
    [Route("/Department", "GET")]
    public class GetAllDepartments : GetAll<Department> { }

    [Route("/Department/{Id}", "GET")]
    public class GetDepartmentById : GetSingleById<Department> { }

    [Route("/Department/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetDepartmentWhere : GetSingleWhere<Department> { }

    [Route("/Department/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedDepartments : GetPaged<Department> { }
    #endregion


    #region Generic Write
    [Route("/Department/CreateInstance", "POST")]
    public class CreateDepartmentInstance : Department { }

    [Route("/Department", "POST")]
    public class InsertDepartment : Department { }

    [Route("/Department", "PUT")]
    public class UpdateDepartment : Department { }

    [Route("/Department/{Id}", "DELETE")]
    public class DeleteDepartment : Department { }
    #endregion

}
