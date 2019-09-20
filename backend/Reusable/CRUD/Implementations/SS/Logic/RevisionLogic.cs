using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class RevisionLogic : WriteLogic<Revision>
    {
        public TrackLogic TrackLogic { get; set; }
        public override void Init(IDbConnection db, IAuthSession auth, IRequest request)
        {
            base.Init(db, auth, request);
            TrackLogic.Init(db, auth, request);
        }

        #region Overrides
        public override Revision Add(Revision entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;

            var track = new Track
            {
                CreatedAt = entity.CreatedAt,
                Discriminator = "Revision",
                CreatedBy = Auth.UserName
            };
            TrackLogic.Add(track);
            entity.TrackId = track.Id;

            return base.Add(entity);
        }

        public override async Task<Revision> AddAsync(Revision entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;

            var track = new Track
            {
                CreatedAt = entity.CreatedAt,
                Discriminator = "Revision",
                CreatedBy = Auth.UserName
            };
            await TrackLogic.AddAsync(track);
            entity.TrackId = track.Id;

            return await base.AddAsync(entity);
        }
        #endregion

        #region Specific Operations
        public List<Revision> GetRevisionsForEntity(long ForeignKey, string ForeignType)
        {
            var query = Db.From<Revision>()
                .Where(e => e.ForeignKey == ForeignKey && e.ForeignType == ForeignType)
                .OrderByDescending(e => e.CreatedAt);

            return Db.LoadSelect(query).ToList();
        }
        #endregion
    }
}
