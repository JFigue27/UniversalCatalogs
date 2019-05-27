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
    [Authenticate]
    public class ItemService : Service
    {
        public IItemLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllItems request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetItemById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetItemWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedItems request)
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
        public object Post(CreateItemInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Item>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertItem request)
        {
            var entity = request.ConvertTo<Item>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateItem request)
        {
            var entity = request.ConvertTo<Item>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteItem request)
        {
            var entity = request.ConvertTo<Item>();
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
    [Route("/Item", "GET")]
    public class GetAllItems : GetAll<Item> { }

    [Route("/Item/{Id}", "GET")]
    public class GetItemById : GetSingleById<Item> { }

    [Route("/Item/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetItemWhere : GetSingleWhere<Item> { }

    [Route("/Item/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedItems : GetPaged<Item> { }
    #endregion

    #region Generic Write
    [Route("/Item/CreateInstance", "POST")]
    public class CreateItemInstance : Item { }

    [Route("/Item", "POST")]
    public class InsertItem : Item { }

    [Route("/Item", "PUT")]
    public class UpdateItem : Item { }

    [Route("/Item/{Id}", "DELETE")]
    public class DeleteItem : Item { }
    #endregion
}
