using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories.Base;
using SmartCard.Infrastructure.Data;

namespace SmartCard.Infrastructure.Repositories.Base;

public class Repository<TEntity, TPrimaryKey>(AppDbContext context)
    : IRepository<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
{
    public async Task<IQueryable<TEntity>> GetAllAsync()
    {
        return await Task.FromResult(context.Set<TEntity>());
    }
    
    public async Task<TEntity?> GetAsync(TPrimaryKey id, CancellationToken cancellationToken)
    {
        return await context.Set<TEntity>()
            .FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);
    }
    
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TEntity?> GetIncludeAsync(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
    }

    public async Task<TEntity?> GetIncludeAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        return await query.FirstOrDefaultAsync(expression);
    }
    
    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        // context.Attach(entity);
        // context.Entry(entity).State = EntityState.Modified;
        // context.Entry(entity).Property(x => x.Id).IsModified = true;
        context.Update(entity);
    }
    
    public async Task<TPrimaryKey> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken)
    {
        var entity = await GetIncludeAsync(id) ?? throw new UserFriendlyException(HttpStatusCode.NotFound, "Entity not found");
        entity.DeletedAt = DateTime.UtcNow;
        Update(entity);
        return id;
    }

    public async Task<TPrimaryKey> HardDeleteAsync(TPrimaryKey id, CancellationToken cancellationToken)
    {
        var entity = await GetIncludeAsync(id) ?? throw new UserFriendlyException(HttpStatusCode.NotFound, "Entity not found");
        context.Set<TEntity>().Remove(entity);
        return id;
    }
    
    
    
    protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
    {
        var parameterExpression = Expression.Parameter(typeof (TEntity));
        var left = Expression.PropertyOrField(parameterExpression, "Id");
        var idValue = Convert.ChangeType(id, typeof (TPrimaryKey));
        var right = Expression.Convert(((LambdaExpression) (() => idValue)).Body, left.Type);
        return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameterExpression);
    }
    
    // protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
    // {
    //     ParameterExpression parameterExpression = Expression.Parameter(typeof (TEntity));
    //     MemberExpression left = Expression.PropertyOrField((Expression) parameterExpression, "Id");
    //     object idValue = Convert.ChangeType((object) id, typeof (TPrimaryKey));
    //     UnaryExpression right = Expression.Convert(((LambdaExpression) (() => idValue)).Body, left.Type);
    //     return Expression.Lambda<Func<TEntity, bool>>((Expression) Expression.Equal((Expression) left, (Expression) right), parameterExpression);
    // }
}