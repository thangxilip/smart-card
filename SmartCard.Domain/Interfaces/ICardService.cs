using SmartCard.Domain.Entities;
using SmartCard.Domain.Enums;

namespace SmartCard.Domain.Interfaces;

public interface ICardService
{
    Task<Card> CreateCardAsync(Guid topicId, string front, string back, CancellationToken cancellationToken);
    Task<IEnumerable<Card>> GetDueCardsAsync(Guid topicId, Guid userId, int limit = 20);
    Task<Card> ReviewCardAsync(Guid cardId, CardRating rating, CancellationToken cancellationToken);
}