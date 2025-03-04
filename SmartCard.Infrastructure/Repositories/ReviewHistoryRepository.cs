using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Repositories;

public class ReviewHistoryRepository(AppDbContext context) : Repository<ReviewHistory, Guid>(context), IReviewHistoryRepository
{
    
}