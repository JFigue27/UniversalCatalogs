using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Reusable.CRUD.Implementations.SS
{
    public class DocumentLogic<Entity> : WriteLogic<Entity>, IDocumentLogic<Entity>, IDocumentLogicAsync<Entity> where Entity : BaseDocument, new()
    {
        public RevisionLogic RevisionLogic { get; set; }
        public TrackLogic TrackLogic { get; set; }

        public override void Init(IDbConnection db, IAuthSession auth, IRequest request)
        {
            base.Init(db, auth, request);
            RevisionLogic.Init(db, auth, request);
            TrackLogic.Init(db, auth, request);
        }

        #region HOOKS
        virtual protected void OnFinalize(Entity entity) { }
        virtual protected void OnUnfinalize(Entity entity) { }
        virtual protected void OnBeforeRevision(Entity clone) { }

        protected override SqlExpression<Entity> OnGetList(SqlExpression<Entity> query)
        {
            return query
                .Where(e => !e.IsDeleted)
                .LeftJoin<Track>();
        }

        protected override SqlExpression<Entity> OnGetSingle(SqlExpression<Entity> query)
        {
            return query
                .Where(e => !e.IsDeleted)
                .LeftJoin<Track>();
        }
        #endregion

        public override Entity Add(Entity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;

            var track = new Track
            {
                CreatedAt = entity.CreatedAt,
                Discriminator = typeof(Entity).Name,
                CreatedBy = Auth.UserName
            };
            TrackLogic.Add(track);
            entity.TrackId = track.Id;

            return base.Add(entity);
        }

        public override async Task<Entity> AddAsync(Entity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;

            var track = new Track
            {
                CreatedAt = entity.CreatedAt,
                Discriminator = typeof(Entity).Name,
                CreatedBy = Auth.UserName
            };
            await TrackLogic.AddAsync(track);
            entity.TrackId = track.Id;

            return await base.AddAsync(entity);
        }

        public override void Remove(Entity entity)
        {
            OnBeforeRemoving(entity);

            var track = TrackLogic.GetById(entity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                TrackLogic.Add(track);
            }
            track.RemovedAt = DateTimeOffset.Now;
            track.RemovedBy = Auth.UserName;
            TrackLogic.Update(track);

            Db.Update<Entity>(new { IsDeleted = true, TrackId = track.Id }, e => e.Id == entity.Id);

            CacheOnDelete(entity);
        }

        public override async Task RemoveAsync(Entity entity)
        {
            OnBeforeRemoving(entity);

            var track = await TrackLogic.GetByIdAsync(entity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                await TrackLogic.AddAsync(track);
            }
            track.RemovedAt = DateTimeOffset.Now;
            track.RemovedBy = Auth.UserName;
            await TrackLogic.UpdateAsync(track);

            await Db.UpdateAsync<Entity>(new { IsDeleted = true, TrackId = track.Id }, e => e.Id == entity.Id);

            CacheOnDelete(entity);
        }

        virtual public void Finalize(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError($"Document: [{typeof(Entity).Name}] does not exist in database.");

            if (originalEntity.DocumentStatus == "FINALIZED")
                throw new KnownError("Document was already Finalized.");
            #endregion

            #region OnFinalize Hook
            OnFinalize(entity);
            #endregion

            #region Finalization
            entity.DocumentStatus = "FINALIZED";
            entity.IsLockedOut = true;

            var track = TrackLogic.GetById(originalEntity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                TrackLogic.Add(track);
            }
            track.UpdatedAt = DateTimeOffset.Now;
            track.UpdatedBy = Auth.UserName;
            TrackLogic.Update(track);
            entity.TrackId = track.Id;

            entity.RevisionMessage = "Finalized";
            Db.Save(entity);
            #endregion

            #region Make Revision
            MakeRevision(entity);
            #endregion

            AdapterOut(entity);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public async Task FinalizeAsync(Entity entity)
        {
            var originalEntity = await GetByIdAsync(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(typeof(Entity).Name));

            if (originalEntity.DocumentStatus == "FINALIZED")
                throw new KnownError("Document was already Finalized.");
            #endregion

            #region OnFinalize Hook
            OnFinalize(entity);
            #endregion

            #region Finalization
            entity.DocumentStatus = "FINALIZED";
            entity.IsLockedOut = true;

            var track = await TrackLogic.GetByIdAsync(originalEntity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                await TrackLogic.AddAsync(track);
            }
            track.UpdatedAt = DateTimeOffset.Now;
            track.UpdatedBy = Auth.UserName;
            await TrackLogic.UpdateAsync(track);
            entity.TrackId = track.Id;

            entity.RevisionMessage = "FINALIZED";
            await Db.SaveAsync(entity);
            #endregion

            #region Make Revision
            await MakeRevisionAsync(entity);
            #endregion

            AdapterOut(entity);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public void Unfinalize(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(typeof(Entity).Name));

            if (originalEntity.DocumentStatus != "FINALIZED")
                throw new KnownError("Document was already Unfinalized.");
            #endregion

            #region OnFinalize Hook
            OnUnfinalize(entity);
            #endregion

            #region UnFinalization
            entity.DocumentStatus = "IN PROGRESS";
            entity.IsLockedOut = false;

            var track = TrackLogic.GetById(originalEntity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                TrackLogic.Add(track);
            }
            track.UpdatedAt = DateTimeOffset.Now;
            track.UpdatedBy = Auth.UserName;
            TrackLogic.Update(track);
            entity.TrackId = track.Id;

            entity.RevisionMessage = "Unfinalized";
            Db.Save(entity);
            #endregion

            #region Make Revision
            MakeRevision(entity);
            #endregion

            AdapterOut(entity);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public async Task UnfinalizeAsync(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(typeof(Entity).Name));

            if (originalEntity.DocumentStatus != "FINALIZED")
                throw new KnownError("Document was already UnFinalized.");
            #endregion

            #region OnFinalize Hook
            OnUnfinalize(entity);
            #endregion

            #region UnFinalization
            entity.DocumentStatus = "IN PROGRESS";
            entity.IsLockedOut = false;

            var track = await TrackLogic.GetByIdAsync(originalEntity.TrackId ?? 0);
            if (track == null)
            {
                track = new Track
                {
                    CreatedAt = entity.CreatedAt,
                    Discriminator = typeof(Entity).Name,
                    CreatedBy = Auth.UserName
                };
                await TrackLogic.AddAsync(track);
            }
            track.UpdatedAt = DateTimeOffset.Now;
            track.UpdatedBy = Auth.UserName;
            await TrackLogic.UpdateAsync(track);
            entity.TrackId = track.Id;

            entity.RevisionMessage = "Unfinalized";
            await Db.SaveAsync(entity);
            #endregion

            #region Make Revision
            await MakeRevisionAsync(entity);
            #endregion

            AdapterOut(entity);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public void MakeRevision(Entity entity)
        {
            //Revision itself:
            var clone = entity.Clone() as Entity;
            clone.Revisions = null;

            OnBeforeRevision(clone);

            var revision = new Revision
            {
                ForeignType = typeof(Entity).Name,
                ForeignKey = entity.Id,
                Value = clone.ToJson(),
                RevisionMessage = entity.RevisionMessage,
                CreatedAt = DateTimeOffset.Now,
                Discriminator = typeof(Entity).Name
            };

            RevisionLogic.Add(revision);
        }

        virtual public async Task MakeRevisionAsync(Entity entity)
        {
            //Revision itself:
            var clone = entity.Clone() as Entity;
            clone.Revisions = null;

            OnBeforeRevision(clone);

            var revision = new Revision
            {
                ForeignType = typeof(Entity).Name,
                ForeignKey = entity.Id,
                Value = clone.ToJson(),
                RevisionMessage = entity.RevisionMessage,
                CreatedAt = DateTimeOffset.Now,
                Discriminator = typeof(Entity).Name
            };

            await RevisionLogic.AddAsync(revision);
        }

        virtual public Entity UpdateAndMakeRevision(Entity entity)
        {
            Update(entity);
            MakeRevision(entity);
            return entity;
        }

        virtual public async Task<Entity> UpdateAndMakeRevisionAsync(Entity entity)
        {
            await UpdateAsync(entity);
            await MakeRevisionAsync(entity);
            return entity;
        }

        virtual public void Checkout(long id)
        {
            var document = GetById(id) as BaseDocument;

            if (!string.IsNullOrWhiteSpace(document.CheckedoutBy)
                && Auth.UserName != document.CheckedoutBy)
                throw new KnownError($"This document is already Checked Out by: {document.CheckedoutBy}");

            if (document.CheckedoutBy == null)
            {
                Db.UpdateOnly(() => new Entity { CheckedoutBy = Auth.UserName }, e => e.Id == id);

                #region Cache
                document.CheckedoutBy = Auth.UserName;
                CacheOnUpdate(document as Entity);
                #endregion
            }
        }

        virtual public async Task CheckoutAsync(long id)
        {
            var document = await GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(document.CheckedoutBy)
                && Auth.UserName != document.CheckedoutBy)
                throw new KnownError("This document is already Checked Out by: {0}".Fmt(document.CheckedoutBy));

            if (document.CheckedoutBy == null)
            {
                await Db.UpdateOnlyAsync(new Entity { CheckedoutBy = Auth.UserName }, e => e.Id == id);

                #region Cache
                document.CheckedoutBy = Auth.UserName;
                CacheOnUpdate(document);
                #endregion
            }
        }

        virtual public void CancelCheckout(long id)
        {
            var document = GetById(id);

            if (!string.IsNullOrWhiteSpace(document.CheckedoutBy)
                && Auth.UserName != document.CheckedoutBy
                && !HasRoles("Admin"))
                throw new KnownError($"Only User who Checked Out can \"Cancel Checked Out\": {document.CheckedoutBy}");

            Db.UpdateOnly(() => new Entity { CheckedoutBy = null }, e => e.Id == id);

            #region Cache
            document.CheckedoutBy = null;
            CacheOnUpdate(document);
            #endregion
        }

        virtual public async Task CancelCheckoutAsync(long id)
        {
            var document = await GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(document.CheckedoutBy) && Auth.UserName != document.CheckedoutBy
                && !HasRoles("Admin"))
                throw new KnownError($"Only User who Checked Out can \"Cancel Checked Out\": {document.CheckedoutBy}");

            await Db.UpdateOnlyAsync(() => new Entity { CheckedoutBy = null }, e => e.Id == id);

            #region Cache
            document.CheckedoutBy = null;
            CacheOnUpdate(document);
            #endregion
        }

        virtual public void Checkin(Entity entity)
        {
            entity.CheckedoutBy = null;
            //Order is important here:
            MakeRevision(entity);
            Update(entity);
        }

        virtual public async Task CheckinAsync(Entity entity)
        {
            entity.CheckedoutBy = null;
            //Order is important here:
            await MakeRevisionAsync(entity);
            await UpdateAsync(entity);
        }

        virtual public Entity CreateAndCheckout(Entity document)
        {
            var instance = CreateInstance(document);
            var entity = Add(instance);
            Checkout(entity.Id);
            return entity;
        }

        virtual public async Task<Entity> CreateAndCheckoutAsync(Entity document)
        {
            var instance = CreateInstance(document);
            var entity = await AddAsync(instance);
            await CheckoutAsync(entity.Id);
            return entity;
        }
    }
}
