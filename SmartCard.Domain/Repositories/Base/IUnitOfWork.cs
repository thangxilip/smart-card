namespace SmartCard.Domain.Repositories.Base;

public interface IUnitOfWork
{
    public IFlashCardRepository FlashCardRepository { get; }
    public ITopicRepository TopicRepository { get; }
    public IFlashCardStateRepository FlashCardStateRepository { get; }
    public IReviewHistoryRepository ReviewHistoryRepository { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}