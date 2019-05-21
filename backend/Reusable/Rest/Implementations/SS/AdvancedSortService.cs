using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class AdvancedSortService : Service
    {
        public IAdvancedSortLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllAdvancedSorts request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetAdvancedSortById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetAdvancedSortWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedAdvancedSorts request)
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
        public object Post(CreateAdvancedSortInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<AdvancedSort>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }
        public object Post(InsertAdvancedSort request)
        {
            var entity = request.ConvertTo<AdvancedSort>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateAdvancedSort request)
        {
            var entity = request.ConvertTo<AdvancedSort>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteAdvancedSort request)
        {
            var entity = request.ConvertTo<AdvancedSort>();
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
    [Route("/AdvancedSort", "GET")]
    public class GetAllAdvancedSorts : GetAll<AdvancedSort> { }

    [Route("/AdvancedSort/{Id}", "GET")]
    public class GetAdvancedSortById : GetSingleById<AdvancedSort> { }

    [Route("/AdvancedSort/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetAdvancedSortWhere : GetSingleWhere<AdvancedSort> { }

    [Route("/AdvancedSort/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedAdvancedSorts : GetPaged<AdvancedSort> { }
    #endregion


    #region Generic Write
    [Route("/AdvancedSort/CreateInstance", "POST")]
    public class CreateAdvancedSortInstance : AdvancedSort { }

    [Route("/AdvancedSort", "POST")]
    public class InsertAdvancedSort : AdvancedSort { }

    [Route("/AdvancedSort", "PUT")]
    public class UpdateAdvancedSort : AdvancedSort { }

    [Route("/AdvancedSort/{Id}", "DELETE")]
    public class DeleteAdvancedSort : AdvancedSort { }
    #endregion

}
