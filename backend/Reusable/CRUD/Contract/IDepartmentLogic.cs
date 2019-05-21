using Reusable.CRUD.Entities;

namespace Reusable.CRUD.Contract
{
    public interface IDepartmentLogic : ILogicWrite<Department>, ILogicWriteAsync<Department>
    {
    }
}
