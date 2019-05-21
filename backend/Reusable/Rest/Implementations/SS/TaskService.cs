using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class TaskService : Service
    {
        public ITaskLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllTasks request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetTaskById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetTaskWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedTasks request)
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
        public object Post(CreateTaskInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<CRUD.Entities.Task>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertTask request)
        {
            var entity = request.ConvertTo<CRUD.Entities.Task>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateTask request)
        {
            var entity = request.ConvertTo<CRUD.Entities.Task>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteTask request)
        {
            var entity = request.ConvertTo<CRUD.Entities.Task>();
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
    [Route("/Task", "GET")]
    public class GetAllTasks : GetAll<CRUD.Entities.Task> { }

    [Route("/Task/{Id}", "GET")]
    public class GetTaskById : GetSingleById<CRUD.Entities.Task> { }

    [Route("/Task/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetTaskWhere : GetSingleWhere<CRUD.Entities.Task> { }

    [Route("/Task/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedTasks : GetPaged<CRUD.Entities.Task> { }
    #endregion


    #region Generic Write
    [Route("/Task/CreateInstance", "POST")]
    public class CreateTaskInstance : CRUD.Entities.Task { }

    [Route("/Task", "POST")]
    public class InsertTask : CRUD.Entities.Task { }

    [Route("/Task", "PUT")]
    public class UpdateTask : CRUD.Entities.Task { }

    [Route("/Task/{Id}", "DELETE")]
    public class DeleteTask : CRUD.Entities.Task { }
    #endregion

}
