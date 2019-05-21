using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Reusable.CRUD.Implementations.SS
{
    public class DocumentLogic<Entity> : LogicWrite<Entity>, IDocumentLogic<Entity>, IDocumentLogicAsync<Entity> where Entity : BaseDocument, new()
    {
        public IRevisionLogic RevisionLogic { get; set; }
        public ITrackLogic TrackLogic { get; set; }

        public override void SetDb(IDbConnection Db)
        {
            base.SetDb(Db);
            RevisionLogic.SetDb(Db);
            TrackLogic.SetDb(Db);
        }

        #region HOOKS
        virtual protected void OnFinalize(Entity entity) { }
        virtual protected void OnUnfinalize(Entity entity) { }
        virtual protected void OnBeforeRevision(Entity clone) { }

        protected override SqlExpression<Entity> OnGetList(SqlExpression<Entity> query)
        {
            query.LeftJoin<User>((d, u) => d.CheckedoutById == u.Id);
            return query;
        }
        protected override SqlExpression<Entity> OnGetSingle(SqlExpression<Entity> query)
        {
            query.LeftJoin<User>((d, u) => d.CheckedoutById == u.Id);
            return query;
        }
        #endregion

        public override Entity Add(ref Entity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;

            var track = new Track
            {
                CreatedAt = entity.CreatedAt,
                Discriminator = entity.EntityName
            };
            TrackLogic.Add(ref track);
            entity.TrackId = track.Id;

            return base.Add(ref entity);
        }

        public override Task<Entity> AddAsync(Entity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;
            return base.AddAsync(entity);
        }

        virtual public void Finalize(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(entity.EntityName));

            if (originalEntity.DocumentStatus == "FINALIZED")
                throw new KnownError("Document was already Finalized.");
            #endregion

            #region OnFinalize Hook
            OnFinalize(entity);
            #endregion

            #region Finalization
            originalEntity.DocumentStatus = "FINALIZED";
            originalEntity.IsLockedOut = true;

            if (originalEntity.Track == null)
                originalEntity.Track = new Track();

            originalEntity.Track.UpdatedAt = DateTimeOffset.Now;
            originalEntity.Track.UpdatedById = LoggedUser.UserID;

            entity.RevisionMessage = "Finalized";
            Db.Save(originalEntity, references: true);
            #endregion

            #region Make Revision
            MakeRevision(entity);
            #endregion

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public async System.Threading.Tasks.Task FinalizeAsync(Entity entity)
        {
            var originalEntity = await GetByIdAsync(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(entity.EntityName));

            if (originalEntity.DocumentStatus == "FINALIZED")
                throw new KnownError("Document was already Finalized.");
            #endregion

            #region OnFinalize Hook
            OnFinalize(entity);
            #endregion

            #region Finalization
            originalEntity.DocumentStatus = "FINALIZED";
            originalEntity.IsLockedOut = true;

            if (originalEntity.Track == null)
                originalEntity.Track = new Track();

            originalEntity.Track.UpdatedAt = DateTimeOffset.Now;
            originalEntity.Track.UpdatedById = LoggedUser.UserID;

            entity.RevisionMessage = "Finalized";
            await Db.SaveAsync(originalEntity, references: true);
            #endregion

            #region Make Revision
            await MakeRevisionAsync(entity);
            #endregion

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public void Unfinalize(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(entity.EntityName));

            if (originalEntity.DocumentStatus != "Finalized")
                throw new KnownError("Document was already UnFinalized.");
            #endregion

            #region OnFinalize Hook
            OnUnfinalize(entity);
            #endregion

            #region UnFinalization
            originalEntity.DocumentStatus = "IN PROGRESS";
            originalEntity.IsLockedOut = false;

            if (originalEntity.Track == null)
                originalEntity.Track = new Track();

            originalEntity.Track.UpdatedAt = DateTimeOffset.Now;
            originalEntity.Track.UpdatedById = LoggedUser.UserID;

            entity.RevisionMessage = "Unfinalized";
            Db.Save(originalEntity, references: true);
            #endregion

            #region Make Revision
            MakeRevision(entity);
            #endregion

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public async System.Threading.Tasks.Task UnfinalizeAsync(Entity entity)
        {
            var originalEntity = GetById(entity.Id);

            #region Validation
            if (originalEntity == null)
                throw new KnownError("Document: [{0}] does not exist in database.".Fmt(entity.EntityName));

            if (originalEntity.DocumentStatus != "Finalized")
                throw new KnownError("Document was already UnFinalized.");
            #endregion

            #region OnFinalize Hook
            OnUnfinalize(entity);
            #endregion

            #region UnFinalization
            originalEntity.DocumentStatus = "IN PROGRESS";
            originalEntity.IsLockedOut = false;

            if (originalEntity.Track == null)
                originalEntity.Track = new Track();

            originalEntity.Track.UpdatedAt = DateTimeOffset.Now;
            originalEntity.Track.UpdatedById = LoggedUser.UserID;

            entity.RevisionMessage = "Unfinalized";
            await Db.SaveAsync(originalEntity, references: true);
            #endregion

            #region Make Revision
            await MakeRevisionAsync(entity);
            #endregion

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
                ForeignType = entity.EntityName,
                ForeignKey = entity.Id,
                Value = clone.ToJson(),
                RevisionMessage = entity.RevisionMessage,
                CreatedAt = DateTimeOffset.Now
            };

            RevisionLogic.Add(ref revision);
        }

        virtual public async System.Threading.Tasks.Task MakeRevisionAsync(Entity entity)
        {
            //Revision itself:
            var clone = entity.Clone() as Entity;
            clone.Revisions = null;

            OnBeforeRevision(clone);

            var revision = new Revision
            {
                ForeignType = entity.EntityName,
                ForeignKey = entity.Id,
                Value = clone.ToJson(),
                RevisionMessage = entity.RevisionMessage,
                CreatedAt = DateTimeOffset.Now
            };

            await RevisionLogic.AddAsync(revision);
        }

        virtual public Entity UpdateAndMakeRevision(Entity entity)
        {
            Update(entity);
            MakeRevision(entity);
            return entity;
        }

        virtual public async System.Threading.Tasks.Task<Entity> UpdateAndMakeRevisionAsync(Entity entity)
        {
            await UpdateAsync(entity);
            await MakeRevisionAsync(entity);
            return entity;
        }

        virtual public void Checkout(long id)
        {
            var document = GetById(id);

            if (LoggedUser?.UserID == null)
                throw new KnownError("User not signed in or User not registered.");

            if (document.CheckedoutById == null)
            {
                document.CheckedoutById = LoggedUser.UserID;
                Db.UpdateOnly(document, only => only.CheckedoutById);
            }
            else if (document.CheckedoutById > 0
                && LoggedUser.UserID != document.CheckedoutById)
            {
                throw new KnownError("This document is already Checked Out by: {0}".Fmt(document.CheckedoutBy?.Value));
            }

            #region Cache
            CacheOnUpdate(document);
            #endregion
        }

        virtual public async System.Threading.Tasks.Task CheckoutAsync(long id)
        {
            var document = await GetByIdAsync(id);

            if (LoggedUser.UserID == null)
                throw new KnownError("User not signed in or User not registered.");

            if (document.CheckedoutById == null)
            {
                document.CheckedoutById = LoggedUser.UserID;
                await Db.UpdateOnlyAsync(document, only => only.CheckedoutById);
            }
            else if (document.CheckedoutById > 0
                && LoggedUser.UserID != document.CheckedoutById)
            {
                throw new KnownError("This document is already Checked Out by: {0}".Fmt(document.CheckedoutBy?.Value));
            }

            #region Cache
            CacheOnUpdate(document);
            #endregion
        }

        virtual public void CancelCheckout(long id)
        {
            var document = GetById(id);

            if (document.CheckedoutById > 0
                && LoggedUser.UserID != document.CheckedoutById
                && LoggedUser.Role != "Administrator")
                throw new KnownError("Only User who Checked Out can \"Cancel Checked Out\": {0}".Fmt(document.CheckedoutBy?.Value));

            document.CheckedoutById = null;
            Db.UpdateOnly(document, only => only.CheckedoutById);

            #region Cache
            CacheOnUpdate(document);
            #endregion
        }

        virtual public async System.Threading.Tasks.Task CancelCheckoutAsync(long id)
        {
            var document = await GetByIdAsync(id);

            if (document.CheckedoutById > 0
                && LoggedUser.UserID != document.CheckedoutById
                && LoggedUser.Role != "Administrator")
                throw new KnownError("Only User who Checked Out can \"Cancel Checked Out\": {0}".Fmt(document.CheckedoutBy?.Value));

            document.CheckedoutById = null;
            await Db.UpdateOnlyAsync(document, only => only.CheckedoutById);

            #region Cache
            CacheOnUpdate(document);
            #endregion
        }

        virtual public void Checkin(Entity entity)
        {
            entity.CheckedoutById = null;
            entity.CheckedoutBy = null;
            Db.UpdateOnly(entity, only => only.CheckedoutById);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public async System.Threading.Tasks.Task CheckinAsync(Entity entity)
        {
            entity.CheckedoutById = null;
            entity.CheckedoutBy = null;
            await Db.UpdateOnlyAsync(entity, only => only.CheckedoutById);

            #region Cache
            CacheOnUpdate(entity);
            #endregion
        }

        virtual public Entity CreateAndCheckout(Entity document)
        {
            var instance = CreateInstance(document);
            var entity = Add(ref instance);
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
