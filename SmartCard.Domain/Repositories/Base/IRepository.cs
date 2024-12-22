using System.Linq.Expressions;
using SmartCard.Domain.Entities;

namespace SmartCard.Domain.Repositories.Base;

public interface IRepository<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
{
    Task<IQueryable<TEntity>> GetAllAsync();

    Task<TEntity?> GetAsync(TPrimaryKey id, CancellationToken cancellationToken);
    
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

    Task<TEntity?> GetIncludeAsync(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetIncludeAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includes);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);

    void Update(TEntity entity);

    Task<TPrimaryKey> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);

    Task<TPrimaryKey> HardDeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);
}