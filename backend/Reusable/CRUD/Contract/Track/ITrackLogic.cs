using Reusable.CRUD.Entities;
using Reusable.Rest;

namespace Reusable.CRUD.Contract
{
    public interface ITrackLogic : ILogicWrite<Track>, ILogicWriteAsync<Track>
    {
        void AssignResponsible(long trackId, long userId);
    }
}
