using Reusable.CRUD.Contract;

namespace Reusable.Rest
{
    #region Read Only ROUTES / DTOs
    public abstract class GetAll<T> where T : BaseEntity { }

    public abstract class GetSingleById<T> where T : BaseEntity
    {
        public long Id { get; set; }
    }

    public abstract class GetSingleWhere<T> where T : BaseEntity
    {
        public string Property { get; set; }
        public object Value { get; set; }
    }

    public abstract class GetPaged<T> where T : BaseEntity
    {
        public string FilterGeneral { get; set; }
        //public long? FilterUser { get; set; }
        public object Value { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
    }
    #endregion
}
