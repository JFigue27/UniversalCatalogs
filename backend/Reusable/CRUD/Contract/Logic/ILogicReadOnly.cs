using Reusable.Rest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reusable.CRUD.Contract
{
    public interface ILogicReadOnly<Entity> : ILogic where Entity : class
    {
        List<Entity> GetAll();
        Entity GetById(long Id);
        CommonResponse GetPaged(int perPage, int page, string filterGeneral, Expression<Func<Entity, bool>>[] wheres, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] database_wheres);
        Entity GetSingleWhere(string Property, object Value);

        Exception GetOriginalException(Exception ex);

        //List<Entity> GetListWhere(Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres);
        //List<Entity> GetAllByParent<ParentType>(long parentID) where ParentType : BaseEntity;
        //Entity CreateInstance(Entity entity = null);
        //void FillRecursively<R>(R entity) where R : class, Entity, IRecursiveEntity<R>;
        //void FillRecursively<R>(R entity, string customProperty, params Expression<Func<Entity, object>>[] navigationProperties) where R : class, Entity, IRecursiveEntity<R>;
        //List<Entity> NestedToSingleList<R>(R entity, List<Entity> result) where R : class, Entity, IRecursiveEntity<R>;
    }

    public interface ILogicReadOnlyAsync<Entity> : ILogic where Entity : class
    {
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(long id);
        Task<CommonResponse> GetPagedAsync(int perPage, int page, string filterGeneral, Expression<Func<Entity, bool>>[] wheres, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] database_wheres);
        Task<Entity> GetSingleWhereAsync(string Property, object Value);
    }
}
