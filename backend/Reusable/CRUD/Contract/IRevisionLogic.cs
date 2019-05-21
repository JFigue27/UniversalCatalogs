using Reusable.CRUD.Entities;
using System.Collections.Generic;

namespace Reusable.CRUD.Contract
{
    public interface IRevisionLogic : ILogicWrite<Revision>, ILogicWriteAsync<Revision>
    {
        List<Revision> GetRevisionsForEntity(long ForeignKey, string ForeignType);
    }
}
