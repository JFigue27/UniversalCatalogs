using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class RevisionService : Service
    {
        public IRevisionLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllRevisions request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetRevisionById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetRevisionWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedRevisions request)
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
        public object Post(CreateRevisionInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Revision>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertRevision request)
        {
            var entity = request.ConvertTo<Revision>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateRevision request)
        {
            var entity = request.ConvertTo<Revision>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteRevision request)
        {
            var entity = request.ConvertTo<Revision>();
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
    [Route("/Revision", "GET")]
    public class GetAllRevisions : GetAll<Revision> { }

    [Route("/Revision/{Id}", "GET")]
    public class GetRevisionById : GetSingleById<Revision> { }

    [Route("/Revision/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetRevisionWhere : GetSingleWhere<Revision> { }

    [Route("/Revision/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedRevisions : GetPaged<Revision> { }
    #endregion


    #region Generic Write
    [Route("/Revision/CreateInstance", "POST")]
    public class CreateRevisionInstance : Revision { }

    [Route("/Revision", "POST")]
    public class InsertRevision : Revision { }

    [Route("/Revision", "PUT")]
    public class UpdateRevision : Revision { }

    [Route("/Revision/{Id}", "DELETE")]
    public class DeleteRevision : Revision { }
    #endregion

}
