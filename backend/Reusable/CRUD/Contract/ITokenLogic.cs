using Reusable.CRUD.Entities;

namespace Reusable.CRUD.Contract
{
    public interface ITokenLogic : ILogicWrite<Token>, ILogicWriteAsync<Token>
    {
    }
}
