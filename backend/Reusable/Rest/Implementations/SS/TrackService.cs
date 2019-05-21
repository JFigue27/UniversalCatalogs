using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class TrackService : Service
    {
        public ITrackLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllTracks request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetTrackById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetTrackWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedTracks request)
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
        public object Post(CreateTrackInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(Logic.CreateInstance(entity));
        }

        public object Post(InsertTrack request)
        {
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(Logic.Add(ref entity));
        }

        public object Put(UpdateTrack request)
        {
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(Logic.Update(entity));
        }
        public object Delete(DeleteTrack request)
        {
            var entity = request.ConvertTo<Track>();
            Logic.Remove(entity);
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        public object Post(AssignResponsible request)
        {
            Logic.AssignResponsible(request.TrackId, request.UserId);
            return new CommonResponse();
        }
        #endregion

    }

    #region Specific
    [Route("/Track/AssignResponsible", "POST")]
    [Route("/Track/AssignResponsible/{TrackId}/{UserId}", "POST")]
    public class AssignResponsible
    {
        public long TrackId { get; set; }
        public long UserId { get; set; }
    }
    #endregion



    #region Generic Read Only
    [Route("/Track", "GET")]
    public class GetAllTracks : GetAll<Track> { }

    [Route("/Track/{Id}", "GET")]
    public class GetTrackById : GetSingleById<Track> { }

    [Route("/Track/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetTrackWhere : GetSingleWhere<Track> { }

    [Route("/Track/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedTracks : GetPaged<Track> { }
    #endregion


    #region Generic Write
    [Route("/Track/CreateInstance", "POST")]
    public class CreateTrackInstance : Track { }

    [Route("/Track", "POST")]
    public class InsertTrack : Track { }

    [Route("/Track", "PUT")]
    public class UpdateTrack : Track { }

    [Route("/Track/{Id}", "DELETE")]
    public class DeleteTrack : Track { }
    #endregion

}
