using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Domain.Repositories;

public interface IFlashCardRepository : IRepository<Card, Guid>
{
    Task<IEnumerable<Card>> GetDueCardsAsync(Guid topicId, Guid userId, int limit = 20);
}