using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Domain.Repositories;

public interface ICardRepository : IRepository<Card, Guid>
{
    
}