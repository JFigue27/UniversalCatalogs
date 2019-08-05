using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS.Logic;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class TrackService : Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public TrackLogic logic { get; set; }

        protected object WithDB(Func<IDbConnection, object> operation)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                logic.Init(db, GetSession(), Request);
                return operation(db);
            }
        }

        protected object InTransaction(Func<IDbConnection, object> operation)
        {
            return WithDB(db =>
            {
                using (var transaction = db.OpenTransaction())
                {
                    try
                    {
                        var result = operation(db);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });
        }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllTracks request)
        {
            return await logic.GetAllAsync();
        }

        public async Task<object> Get(GetTrackById request)
        {
            return await logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetTrackWhere request)
        {
            return await logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedTracks request)
        {
            return await logic.GetPagedAsync(
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
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(logic.CreateInstance(entity));
        }

        public object Post(InsertTrack request)
        {
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(logic.Add(entity));
        }

        public object Put(UpdateTrack request)
        {
            var entity = request.ConvertTo<Track>();
            return new CommonResponse(logic.Update(entity));
        }
        public object Delete(DeleteTrack request)
        {
            var entity = request.ConvertTo<Track>();
            logic.Remove(entity);
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        public object Post(Assign request)
        {
            //InTransaction(db => logic.AssignResponsible(request.TrackId, request.UserName));
            return new CommonResponse();
        }
        #endregion

    }

    #region Specific
    [Route("/Track/Assign", "POST")]
    [Route("/Track/Assign/{TrackId}/{UserName}", "POST")]
    public class Assign
    {
        public long TrackId { get; set; }
        public string UserName { get; set; }
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
