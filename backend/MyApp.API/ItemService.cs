using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;

namespace MyApp.API
{
    // [Authenticate]
    public class ItemService : BaseService<ItemLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllItems request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetItemById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetItemWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedItems request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateItemInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Item>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertItem request)
        {
            var entity = request.ConvertTo<Item>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateItem request)
        {
            var entity = request.ConvertTo<Item>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteItem request)
        {
            var entity = request.ConvertTo<Item>();
            return InTransaction(db => {
                Logic.Remove(entity);
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
    [Route("/Item", "GET")]
    public class GetAllItems : GetAll<Item> { }

    [Route("/Item/{Id}", "GET")]
    public class GetItemById : GetSingleById<Item> { }

    [Route("/Item/GetSingleWhere", "GET")]
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

    [Route("/Item", "DELETE")]
    [Route("/Item/{Id}", "DELETE")]
    public class DeleteItem : Item { }
    #endregion
}
