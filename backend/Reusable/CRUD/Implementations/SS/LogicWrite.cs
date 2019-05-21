using Reusable.CRUD.Contract;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reusable.CRUD.Implementations.SS
{
    public class LogicWrite<Entity> : ReadOnlyLogic<Entity>, ILogicWrite<Entity>, ILogicWriteAsync<Entity> where Entity : BaseEntity, new()
    {
        #region HOOKS
        protected enum OPERATION_MODE { NONE, ADD, UPDATE };
        virtual protected Entity OnCreateInstance(Entity entity) { return entity; }
        virtual protected void OnAfterSaving(Entity entity, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        virtual protected void OnBeforeSaving(Entity entity, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        virtual protected void OnBeforeRemoving(Entity entity) { }
        virtual protected void OnBeforeRemoving(long id) { }

        virtual protected async Task OnAfterSavingAsync(Entity entity, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        virtual protected async Task OnBeforeSavingAsync(Entity entity, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        virtual protected async Task OnBeforeRemovingAsync(Entity entity) { }
        virtual protected async Task OnBeforeRemovingAsync(long id) { }
        #endregion

        virtual public Entity CreateInstance(Entity entity = null)
        {
            if (entity == null) entity = new Entity();

            return OnCreateInstance(entity);
        }

        virtual public Entity Add(ref Entity entity)
        {
            OnBeforeSaving(entity, OPERATION_MODE.ADD);

            entity.Id = Db.Insert(entity, selectIdentity: true);

            OnAfterSaving(entity, OPERATION_MODE.ADD);

            CacheOnAdd(entity);

            return entity;
        }

        virtual public async Task<Entity> AddAsync(Entity entity)
        {
            OnBeforeSaving(entity, OPERATION_MODE.ADD);

            entity.Id = await Db.InsertAsync(entity, selectIdentity: true);

            OnAfterSaving(entity, OPERATION_MODE.ADD);

            CacheOnAdd(entity);

            return entity;
        }

        virtual public Entity Update(Entity entity)
        {
            OnBeforeSaving(entity, OPERATION_MODE.UPDATE);

            Db.Update(entity);

            OnAfterSaving(entity, OPERATION_MODE.UPDATE);

            CacheOnUpdate(entity);

            return entity;
        }

        virtual public async Task<Entity> UpdateAsync(Entity entity)
        {
            OnBeforeSaving(entity, OPERATION_MODE.UPDATE);

            await Db.UpdateAsync(entity);

            OnAfterSaving(entity, OPERATION_MODE.UPDATE);

            CacheOnUpdate(entity);

            return entity;
        }

        virtual public void Remove(Entity entity)
        {
            OnBeforeRemoving(entity);

            Db.Delete(entity);

            CacheOnDelete(entity);
        }

        virtual public async Task RemoveAsync(Entity entity)
        {
            OnBeforeRemoving(entity);

            await Db.DeleteAsync(entity);

            CacheOnDelete(entity);
        }

        virtual public void RemoveAll()
        {
            Db.DeleteAll<Entity>();

            CacheOnDeleteAll();
        }

        #region Cache
        virtual protected void CacheOnAdd(Entity entity)
        {
            entity.RowVersion = Db.SingleById<Entity>(entity.Id).RowVersion;

            var cacheGetAll = Cache.Get<List<Entity>>(CacheGetAllKey);
            if (cacheGetAll != null)
            {
                cacheGetAll.Add(entity);
                Cache.Replace(CacheGetAllKey, cacheGetAll);
            }

            var cacheGetById = Cache.Get<object>(CacheGetByIdKey + "_" + entity.Id);
            if (cacheGetById != null)
                Cache.Replace(CacheGetByIdKey + "_" + entity.Id, entity);

            Cache.Remove(CacheContainerGetPagedKey);
            Cache.Remove(CacheContainerGetSingleWhereKey);
        }

        virtual protected void CacheOnUpdate(Entity entity)
        {
            entity.RowVersion = Db.SingleById<Entity>(entity.Id).RowVersion;

            var cacheGetAll = Cache.Get<List<Entity>>(CacheGetAllKey);
            if (cacheGetAll != null)
            {
                if (cacheGetAll.Exists(e => e.Id == entity.Id))
                {
                    var toUpdate = cacheGetAll.FindIndex(e => e.Id == entity.Id);
                    cacheGetAll[toUpdate] = entity;
                }
                else
                {
                    cacheGetAll.Add(entity);
                }
                Cache.Replace(CacheGetAllKey, cacheGetAll);
            }

            var cacheGetById = Cache.Get<object>(CacheGetByIdKey + "_" + entity.Id);
            if (cacheGetById != null)
                Cache.Replace(CacheGetByIdKey + "_" + entity.Id, entity);

            Cache.Remove(CacheContainerGetPagedKey);
            Cache.Remove(CacheContainerGetSingleWhereKey);
        }

        virtual protected void CacheOnDelete(Entity entity)
        {
            var cacheGetAll = Cache.Get<List<Entity>>(CacheGetAllKey);
            if (cacheGetAll != null)
            {
                var toRemove = cacheGetAll.Find(e => e.Id == entity.Id);
                cacheGetAll.Remove(toRemove);
                Cache.Replace(CacheGetAllKey, cacheGetAll);
            }

            Cache.Remove(CacheGetByIdKey + "_" + entity.Id);

            Cache.Remove(CacheContainerGetPagedKey);
            Cache.Remove(CacheContainerGetSingleWhereKey);
        }

        virtual protected void CacheOnDeleteAll()
        {
            //ToDO: Remove only concerning to Current Entity.
            Cache.FlushAll();
        }
        #endregion
    }
}
