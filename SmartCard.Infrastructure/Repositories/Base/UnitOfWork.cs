using SmartCard.Domain.Repositories;
using SmartCard.Domain.Repositories.Base;
using SmartCard.Infrastructure.Data;

namespace SmartCard.Infrastructure.Repositories.Base;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public ICardRepository CardRepository { get; } = new CardRepository(context);
    
    public ITopicRepository TopicRepository { get; } = new TopicRepository(context);
    
    public int SaveChanges() => context.SaveChanges();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}