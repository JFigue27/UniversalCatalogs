using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace MyApp.API
{
    // [Authenticate]
    public class AreaService : Service
    {
        public IAreaLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllAreas request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetAreaById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetAreaWhere request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedAreas request)
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
        public object Post(CreateAreaInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Area>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertArea request)
        {
            var entity = request.ConvertTo<Area>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateArea request)
        {
            var entity = request.ConvertTo<Area>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteArea request)
        {
            var entity = request.ConvertTo<Area>();
            InTransaction(() => Logic.Remove(entity));
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion

        private void InTransaction(params Action[] Operations)
        {
            Logic.SetDb(Db);
            Logic.SetAuth(GetSession());
            using (var transaction = Db.OpenTransaction())
            {
                try
                {
                    foreach (var operation in Operations)
                    {
                        operation();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    #region Specific
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/Area", "GET")]
    public class GetAllAreas : GetAll<Area> { }

    [Route("/Area/{Id}", "GET")]
    public class GetAreaById : GetSingleById<Area> { }

    [Route("/Area/GetSingleWhere", "GET")]
    [Route("/Area/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetAreaWhere : GetSingleWhere<Area> { }

    [Route("/Area/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedAreas : GetPaged<Area> { }
    #endregion

    #region Generic Write
    [Route("/Area/CreateInstance", "POST")]
    public class CreateAreaInstance : Area { }

    [Route("/Area", "POST")]
    public class InsertArea : Area { }

    [Route("/Area", "PUT")]
    public class UpdateArea : Area { }

    [Route("/Area", "DELETE")]
    [Route("/Area/{Id}", "DELETE")]
    public class DeleteArea : Area { }
    #endregion
}
