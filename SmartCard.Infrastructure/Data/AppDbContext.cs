using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SmartCard.Domain.Entities;
using SmartCard.Domain.Enums;
using SmartCard.Infrastructure.Identity;

namespace SmartCard.Infrastructure.Data;

public sealed class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext()
    {
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        ChangeTracker.StateChanged += UpdateBaseEntity;
        ChangeTracker.Tracked += UpdateBaseEntity;
    }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<FlashCardState> FlashCardStates { get; set; }
    public DbSet<ReviewHistory> Reviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .HasOne(c => c.State)
            .WithOne(f => f.Card)
            .HasForeignKey<FlashCardState>(f => f.CardId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
        ApplyQueryFilter(modelBuilder);
    }
    
    private void UpdateBaseEntity(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is BaseEntity<Guid> baseEntity)
        {
            switch (e.Entry.State)
            {
                case EntityState.Added:
                    baseEntity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
    
    private void ApplyQueryFilter(ModelBuilder modelBuilder)
    {
        var clrTypes = modelBuilder.Model.GetEntityTypes().Select(et => et.ClrType).ToList();
        var softDeleteFilter = (Expression<Func<BaseEntity<Guid>, bool>>)(e => e.DeletedAt == null);

        foreach (var type in clrTypes)
        {
            if (typeof(BaseEntity<Guid>).IsAssignableFrom(type))
            {
                var queryFilter = CombineQueryFilters(type, softDeleteFilter);
                modelBuilder.Entity(type).HasQueryFilter(queryFilter);
            }
        }

        return;

        // https://learn.microsoft.com/en-us/ef/core/querying/filters
        LambdaExpression CombineQueryFilters(Type entityType, LambdaExpression filterEpxression)
        {
            var newParam = Expression.Parameter(entityType);
            Expression? andAlsoExpr = null;

            var expression = ReplacingExpressionVisitor.Replace(filterEpxression.Parameters.Single(), newParam, filterEpxression.Body);
            andAlsoExpr = andAlsoExpr == null ? expression : Expression.AndAlso(andAlsoExpr, expression);

            var exp = andAlsoExpr ?? Expression.Constant(false);

            return Expression.Lambda(exp, newParam);
        }
    }
}