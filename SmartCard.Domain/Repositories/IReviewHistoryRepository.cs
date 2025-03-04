using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Domain.Repositories;

public interface IReviewHistoryRepository : IRepository<ReviewHistory, Guid>
{
    
}