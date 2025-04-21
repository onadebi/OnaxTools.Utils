//using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System.Globalization;
using System.Linq.Expressions;

namespace OnaxTools.Services.DbAccess;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected DbContext context;

    public Repository(DbContext Context)
    {
        context = Context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
    {
        return await Queryable.AsQueryable(context.Set<TEntity>()).Where(predicate).AnyAsync();
    }

    public async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
        //await context.BulkDeleteAsync(entities);
    }

    public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
        //await context.BulkInsertAsync(entities.ToList());
    }

    public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
        //await context.BulkUpdateAsync(entities.ToList());
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Queryable.AsQueryable(context.Set<TEntity>()).CountAsync(predicate);
    }

    public async Task<decimal> DecimalSumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector)
    {
        return await Queryable.AsQueryable(context.Set<TEntity>()).Where(predicate).SumAsync(selector);
    }

    public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await BuildQuery(predicate, order, isDescOrder, skip, pageSize, withoutTraking, includeProperties).ToListAsync();
    }

    public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int? skip, int? pageSize, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAsync(predicate, null, null, skip, pageSize, withoutTraking, includeProperties);
    }

    public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAsync(predicate, null, null, withoutTraking, includeProperties);
    }

    public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAsync(predicate, null, null, withoutTraking: true, includeProperties);
    }

    public async Task<Tuple<IList<TEntity>, int>> FindAsync(List<Expression<Func<TEntity, bool>>> predicates, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool includeTotalCount, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        Tuple<IQueryable<TEntity>, int> query = BuildQueryMultiplePredicates(predicates, order, isDescOrder, skip, pageSize, includeTotalCount, includeProperties);
        return Tuple.Create((IList<TEntity>)(await query.Item1.ToListAsync()), query.Item2);
    }

    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> queryable = Queryable.AsQueryable(context.Set<TEntity>()).Where(predicate);
        if (includeProperties != null && includeProperties.Length > 0)
        {
            queryable = includeProperties.Aggregate(queryable, (IQueryable<TEntity> current, Expression<Func<TEntity, object>> include) => current.Include(include));
        }

        return queryable.FirstOrDefault();
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> queryable = Queryable.AsQueryable(context.Set<TEntity>()).Where(predicate);
        if (includeProperties != null && includeProperties.Length > 0)
        {
            queryable = includeProperties.Aggregate(queryable, (IQueryable<TEntity> current, Expression<Func<TEntity, object>> include) => current.Include(include));
        }

        return await queryable.FirstOrDefaultAsync();
    }

    public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> order = null, bool? isDescOrder = null, int? skip = null, int? pageSize = null)
    {
        return await BuildQuery(null, order, isDescOrder, skip, pageSize, true).ToListAsync();
    }

    public ValueTask<TEntity> GetByIdAsync(int id)
    {
        return context.Set<TEntity>().FindAsync(id);
    }

    public ValueTask<TEntity> GetByIdAsync(string id)
    {
        return context.Set<TEntity>().FindAsync(id);
    }

    public ValueTask<TEntity> GetByIdAsync(decimal id)
    {
        return context.Set<TEntity>().FindAsync(id);
    }

    public ValueTask<TEntity> GetByIdAsync(Guid id)
    {
        return context.Set<TEntity>().FindAsync(id);
    }

    public long GetNextSequenceValue(string name, string schema = null)
    {
        //string sql = context.GetService<IUpdateSqlGenerator>().GenerateNextSequenceValueOperation(name, schema ?? context.Model.GetDefaultSchema());
        //IRelationalCommand relationalCommand = context.GetService<IRawSqlCommandBuilder>().Build(sql);

        //RelationalCommandParameterObject parameterObject = new(
        //    context.GetService<IRelationalConnection>(),
        //    logger: context.GetService<IRelationalCommandDiagnosticsLogger>(),
        //    parameterValues: null,
        //    readerColumns: null,
        //    context: context
        //);
        //return Convert.ToInt64(relationalCommand.ExecuteScalar(parameterObject), CultureInfo.InvariantCulture);
        return 0L;
    }

    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
    }

    public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return context.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }


    #region Helpers
    protected IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool withoutTraking, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> queryable = Queryable.AsQueryable(context.Set<TEntity>());
        if (order != null)
        {
            queryable = ((!isDescOrder.GetValueOrDefault()) ? queryable.OrderBy(order) : queryable.OrderByDescending(order));
        }

        if (includeProperties != null)
        {
            queryable = includeProperties.Aggregate(queryable, (IQueryable<TEntity> current, Expression<Func<TEntity, object>> include) => current.Include(include));
        }

        if (predicate != null)
        {
            queryable = queryable.Where(predicate);
        }

        if (skip.HasValue && pageSize.HasValue)
        {
            queryable = queryable.Skip(skip.Value).Take(pageSize.Value);
        }

        if (withoutTraking)
        {
            queryable = queryable.AsNoTracking();
        }

        return queryable;
    }

    protected Tuple<IQueryable<TEntity>, int> BuildQueryMultiplePredicates(List<Expression<Func<TEntity, bool>>> predicates, Expression<Func<TEntity, object>> order, bool? isDescOrder, int? skip, int? pageSize, bool includeTotalCount, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> queryable = Queryable.AsQueryable(context.Set<TEntity>());
        int item = 0;
        if (order != null)
        {
            queryable = ((!isDescOrder.GetValueOrDefault()) ? queryable.OrderBy(order) : queryable.OrderByDescending(order));
        }

        if (includeProperties != null)
        {
            queryable = includeProperties.Aggregate(queryable, (IQueryable<TEntity> current, Expression<Func<TEntity, object>> include) => current.Include(include));
        }

        if (predicates != null)
        {
            foreach (Expression<Func<TEntity, bool>> predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }
        }

        if (includeTotalCount)
        {
            item = queryable.Count();
        }

        if (skip.HasValue && pageSize.HasValue)
        {
            queryable = queryable.Skip(skip.Value).Take(pageSize.Value);
        }

        return Tuple.Create(queryable, item);
    }
    #endregion
}
