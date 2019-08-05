using System.Collections.Generic;
using System.Linq;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class RevisionLogic : LogicWrite<Revision>
    {
        #region Overrides
        #endregion

        #region Specific Operations
        public List<Revision> GetRevisionsForEntity(long ForeignKey, string ForeignType)
        {
            var query = Db.From<Revision>()
                .LeftJoin<Track>()
                .Where(e => e.ForeignKey == ForeignKey && e.ForeignType == ForeignType)
                .OrderByDescending(e => e.CreatedAt);

            return Db.Select(query).ToList();
        }
        #endregion
    }
}
