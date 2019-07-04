using System.Threading.Tasks;

namespace Reusable.CRUD.Contract
{
    public interface ILogicWrite<Entity> : ILogicReadOnly<Entity> where Entity : class
    {
        //CommonResponse AddTransaction(Entity entity);
        Entity CreateInstance(Entity entity = null);
        Entity Add(ref Entity entity);
        Entity Update(Entity entity);
        void Remove(Entity id);
        void RemoveAll();
        //void Remove(long id);
        //CommonResponse UpdateTransaction(Entity entity);
        //CommonResponse AddToParent<ParentType>(long parentID, Entity entity) where ParentType : BaseEntity;
        //CommonResponse RemoveFromParent<Parent>(long parentID, Entity entity) where Parent : BaseEntity;
        //CommonResponse SetPropertyValue(Entity entity, string sProperty, string value);
        //CommonResponse GetAvailableFor<ForEntity>(long id) where ForEntity : BaseEntity;
    }

    public interface ILogicWriteAsync<Entity> : ILogicReadOnlyAsync<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task<Entity> UpdateAsync(Entity entity);
        Task RemoveAsync(Entity entity);
    }
}
