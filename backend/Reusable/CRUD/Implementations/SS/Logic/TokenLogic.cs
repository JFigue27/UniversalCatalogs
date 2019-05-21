using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using System;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class TokenLogic : LogicWrite<Token>, ITokenLogic
    {
        #region Overrides
        protected override void OnBeforeSaving(Token entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            entity.CreatedAt = DateTimeOffset.Now;
        }
        #endregion

        #region Specific Operations
        #endregion
    }
}
