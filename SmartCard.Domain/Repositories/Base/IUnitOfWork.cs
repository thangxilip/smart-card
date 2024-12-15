namespace SmartCard.Domain.Repositories.Base;

public interface IUnitOfWork
{
    public ICardRepository CardRepository { get; }
    public ITopicRepository TopicRepository { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}