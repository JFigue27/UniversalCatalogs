using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class TrackLogic : LogicWrite<Track>
    {
        #region Overrides
        #endregion

        #region Specific Operations
        public void AssignResponsible(long trackId, string username)
        {
            Db.Update<Track>(new
            {
                AssignedTo = username,
                AssignedBy = Auth.UserName
            }, where => where.Id == trackId);
        }
        #endregion
    }
}
