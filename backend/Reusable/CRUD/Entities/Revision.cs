using Reusable.CRUD.Contract;

namespace Reusable.CRUD.Entities
{
    public class Revision : BaseDocument
    {
        public string ForeignType { get; set; }
        public long ForeignKey { get; set; }
        public string Value { get; set; }
    }
}
