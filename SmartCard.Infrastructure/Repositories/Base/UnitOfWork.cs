using SmartCard.Domain.Repositories;
using SmartCard.Domain.Repositories.Base;
using SmartCard.Infrastructure.Data;

namespace SmartCard.Infrastructure.Repositories.Base;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public ITopicRepository TopicRepository { get; } = new TopicRepository(context);
    public IFlashCardRepository FlashCardRepository { get; } = new FlashCardRepository(context);
    public IFlashCardStateRepository FlashCardStateRepository { get; } = new FlashCardStateRepository(context);
    public IReviewHistoryRepository ReviewHistoryRepository { get; } = new ReviewHistoryRepository(context);
    
    public int SaveChanges() => context.SaveChanges();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}