using Reusable.CRUD.Contract;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace Reusable.CRUD.Entities
{
    public class AdvancedSort : BaseEntity
    {
        public AdvancedSort()
        {
            Sorting = new List<SortData>();
            Filtering = new List<FilterData>();
        }

        public string Name { get; set; }

        [Reference]
        public List<SortData> Sorting { get; set; }

        [Reference]
        public List<FilterData> Filtering { get; set; }

        [Reference]
        public User User { get; set; }
        public long? UserId { get; set; }
    }

    public class SortData : BaseEntity
    {
        public string Value { get; set; }
        public int Sequence { get; set; }
        public string AscDesc { get; set; }

        public long AdvancedSortId { get; set; }
    }

    public class FilterData : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public long AdvancedSortId { get; set; }
    }
}
