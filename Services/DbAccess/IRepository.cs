using System.Linq.Expressions;

namespace OnaxTools.Services.DbAccess;

public interface IRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity> GetByIdAsync(int id);

    ValueTask<TEntity> GetByIdAsync(string id);

    ValueTask<TEntity> GetByIdAsync(decimal id);

    ValueTask<TEntity> GetByIdAsync(Guid id);

    Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> order = null, bool? isDescOrder = null, int? skip = null, int? pageSize = null);

    Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int? skip, int? pageSize, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    Task<decimal> DecimalSumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector);

    Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task BulkInsertAsync(IEnumerable<TEntity> entities);

    Task BulkUpdateAsync(IEnumerable<TEntity> entities);

    Task BulkDeleteAsync(IEnumerable<TEntity> entities);

    long GetNextSequenceValue(string name, string schema = null);

    Task<Tuple<IList<TEntity>, int>> FindAsync(List<Expression<Func<TEntity, bool>>> predicates, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool includeTotalCount, params Expression<Func<TEntity, object>>[] includeProperties);
}
