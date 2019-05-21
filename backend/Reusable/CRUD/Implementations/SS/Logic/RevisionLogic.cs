using System.Collections.Generic;
using System.Linq;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class RevisionLogic : LogicWrite<Revision>, IRevisionLogic
    {
        #region Overrides
        #endregion

        #region Specific Operations
        public List<Revision> GetRevisionsForEntity(long ForeignKey, string ForeignType)
        {
            var query = Db.From<Revision>()
                .LeftJoin<Track>()
                .LeftJoin<Track, User>((t, u) => t.CreatedById == u.Id)
                .Where(e => e.ForeignKey == ForeignKey && e.ForeignType == ForeignType)
                .OrderByDescending(e => e.CreatedAt);

            return Db.Select(query).ToList();
        }
        #endregion
    }
}
