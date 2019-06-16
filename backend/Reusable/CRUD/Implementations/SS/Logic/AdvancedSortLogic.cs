using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;
using System.Linq;
using System.Collections.Generic;
using ServiceStack;
using Reusable.CRUD.Contract;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class AdvancedSortLogic : LogicWrite<AdvancedSort>, IAdvancedSortLogic
    {
        #region Overrides
        protected override SqlExpression<AdvancedSort> OnGetList(SqlExpression<AdvancedSort> query)
        {
            return query
                .LeftJoin<SortData>()
                .LeftJoin<FilterData>();
        }
        protected override SqlExpression<AdvancedSort> OnGetSingle(SqlExpression<AdvancedSort> query)
        {
            return query
                .LeftJoin<SortData>()
                .LeftJoin<FilterData>()
                .Where(e => e.UserName == Auth.UserName);
        }
        protected override IEnumerable<AdvancedSort> AdapterOut(params AdvancedSort[] entities)
        {
            foreach (var item in entities)
            {
                item.Sorting = item.Sorting
                    .OrderBy(e => e.Sequence).ToList();
            }

            return entities;
        }
        protected override void OnBeforeSaving(AdvancedSort entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            entity.UserName = Auth.UserName;

            foreach (var item in entity.Sorting)
            {
                item.AdvancedSortId = entity.Id;
            }
            Db.SaveAll(entity.Sorting);

            foreach (var item in entity.Filtering)
            {
                item.AdvancedSortId = entity.Id;
            }
            Db.SaveAll(entity.Filtering);

            if (mode == OPERATION_MODE.UPDATE)
            {
                var originalEntity = GetById(entity.Id);

                for (int i = originalEntity.Sorting.Count - 1; i >= 0; i--)
                {
                    var oSort = originalEntity.Sorting[i];
                    if (!entity.Sorting.Any(e => e.Id == oSort.Id))
                    {
                        Db.Delete(oSort);
                    }
                }

                for (int i = originalEntity.Filtering.Count - 1; i >= 0; i--)
                {
                    var oFilter = originalEntity.Filtering[i];
                    if (!entity.Filtering.Any(e => e.Id == oFilter.Id))
                    {
                        Db.Delete(oFilter);
                    }
                }
            }
        }
        #endregion

        #region Specific Operations
        #endregion
    }
}
