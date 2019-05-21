using Reusable.CRUD.Entities;
using Reusable.CRUD.JsonEntities;
using System.Collections.Generic;

namespace Reusable.CRUD.Contract
{
    public interface ITaskLogic : ILogicWrite<Task>, ILogicWriteAsync<Task>
    {
        void SaveTasks(List<Contact> responsibles, BaseEntity fromEntity = null);
        void SaveTask(Contact responsible, BaseEntity fromEntity = null);
        void SaveTask(long responsibleId, BaseEntity fromEntity = null);
        void CancelTask(BaseEntity fromEntity);
        Task GetTaskFromEntity(BaseEntity fromEntity, long? userKey);
        //void TransferTasks(int fromUserKey, int toUserKey);
    }
}
