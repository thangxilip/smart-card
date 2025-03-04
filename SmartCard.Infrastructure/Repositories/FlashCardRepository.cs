using Microsoft.EntityFrameworkCore;
using SmartCard.Domain.Entities;
using SmartCard.Domain.Enums;
using SmartCard.Domain.Repositories;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Repositories;

public class FlashCardRepository(AppDbContext context) : Repository<Card, Guid>(context), IFlashCardRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Card>> GetDueCardsAsync(Guid topicId, Guid userId, int limit = 20)
    {
        // var now = DateTime.UtcNow;
        // return await _context.Cards
        //     .Include(c => c.State)
        //     .Where(c => c.TopicId == topicId 
        //                 && c.State.CreatedBy == userId
        //                 && (c.State.Status == CardState.New
        //                     || c.State.NextReviewAt <= now))
        //     .OrderBy(c => c.State.Status == CardState.New? 0 : 1)
        //     .ThenBy(c => c.State.NextReviewAt)
        //     .Take(limit)
        //     .ToListAsync();
        return default;
    }
    // public async Task<IQueryable<FlashCard>> GetStudyCardsByTopic(Guid topicId, CancellationToken cancellationToken)
    // {
    //     var cards = context.Cards.Where(x => x.TopicId == topicId &&
    //                                         (!x.StartedStudying || x.NextStudyDate <= DateOnly.FromDateTime(DateTime.UtcNow)));
    //
    //     return await Task.FromResult(cards);
    // }
}