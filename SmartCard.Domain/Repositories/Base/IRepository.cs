using System.Linq.Expressions;
using SmartCard.Domain.Entities;

namespace SmartCard.Domain.Repositories.Base;

public interface IRepository<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
{
    IQueryable<TEntity> GetAll();
    
    IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity?> GetAsync(TPrimaryKey id, CancellationToken cancellationToken);
    
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

    Task<TEntity?> GetIncludeAsync(TPrimaryKey id, params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<TEntity?> GetIncludeAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);

    void Update(TEntity entity);

    Task<TPrimaryKey> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);

    Task<TPrimaryKey> HardDeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);
}