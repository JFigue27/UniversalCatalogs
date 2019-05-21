using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class TrackLogic : LogicWrite<Track>, ITrackLogic
    {
        #region Overrides
        #endregion

        #region Specific Operations
        public void AssignResponsible(long trackId, long userId)
        {
            Db.Update<Track>(new
            {
                User_AssignedToKey = userId,
                User_AssignedByKey = LoggedUser.UserID
            }, where => where.Id == trackId);
        }
        #endregion
    }
}
