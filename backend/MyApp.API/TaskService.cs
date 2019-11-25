using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;
using Task = MyApp.Logic.Entities.Task;
///start:slot:imports<<<///end:slot:imports<<<

namespace MyApp.API
{
    // [Authenticate]
    public class TaskService : BaseService<TaskLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllTasks request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetTaskById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetTaskWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedTasks request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateTaskInstance request)
        {
            return WithDb(db =>
            {
                var entity = request.ConvertTo<Task>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertTask request)
        {
            var entity = request.ConvertTo<Task>();
            return InTransaction(db =>
            {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateTask request)
        {
            var entity = request.ConvertTo<Task>();
            return InTransaction(db =>
            {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteTask request)
        {
            var entity = request.ConvertTo<Task>();
            return InTransaction(db =>
            {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        public object Delete(DeleteByIdTask request)
        {
            var entity = request.ConvertTo<Task>();
            return InTransaction(db =>
            {
                Logic.RemoveById(entity.Id);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Specific
        
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion
    }

    #region Specific
    
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/Task", "GET")]
    public class GetAllTasks : GetAll<Task> { }

    [Route("/Task/{Id}", "GET")]
    public class GetTaskById : GetSingleById<Task> { }

    [Route("/Task/GetSingleWhere", "GET")]
    [Route("/Task/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetTaskWhere : GetSingleWhere<Task> { }

    [Route("/Task/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedTasks : GetPaged<Task> { }
    #endregion

    #region Generic Write
    [Route("/Task/CreateInstance", "POST")]
    public class CreateTaskInstance : Task { }

    [Route("/Task", "POST")]
    public class InsertTask : Task { }

    [Route("/Task", "PUT")]
    public class UpdateTask : Task { }

    [Route("/Task", "DELETE")]
    public class DeleteTask : Task { }

    [Route("/Task/{Id}", "DELETE")]
    public class DeleteByIdTask : Task { }
    #endregion
}
