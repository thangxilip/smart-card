using SmartCard.Domain.Entities;
using SmartCard.Domain.Repositories;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Repositories;

public class CardRepository(AppDbContext context) : Repository<Card, Guid>(context), ICardRepository
{
    
}