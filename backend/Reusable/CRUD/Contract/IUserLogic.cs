using Reusable.CRUD.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reusable.CRUD.Contract
{
    public interface IRoleLogic : ILogicWrite<Role>, ILogicWriteAsync<Role> { }

    public interface IUserLogic : IDocumentLogic<User>, IDocumentLogicAsync<User> {
        User GetByUserName(string UserName);
        List<User> GetByRole(string Role);

        Task<User> GetByUserNameAsync(string UserName);
        Task<List<User>> GetByRoleAsync(string Role);
    }
}
