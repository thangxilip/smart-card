using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Repositories;

public class CardRepository(AppDbContext context) : Repository<Card, Guid>(context), ICardRepository
{
    public async Task<IQueryable<Card>> GetStudyCardsByTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var cards = context.Cards.Where(x => x.TopicId == topicId &&
                                            (!x.StartedStudying || x.NextStudyDate <= DateOnly.FromDateTime(DateTime.UtcNow)));

        return await Task.FromResult(cards);
    }
}