using Microsoft.EntityFrameworkCore;
using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Repositories;

public class TopicRepository(AppDbContext context) : Repository<Topic, Guid>(context), ITopicRepository
{
    public async Task<Topic?> GetIncludeStatisticsAsync(Guid id)
    {
        var topic = await context.Topics
            .Include(x => x.Cards)!
            .ThenInclude(x => x.State)
            .FirstOrDefaultAsync(x => x.Id == id);

        return topic;
    }
}