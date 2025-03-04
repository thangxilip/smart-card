using MediatR;
using SmartCard.Domain.Interfaces;

namespace SmartCard.Application.Domains.Card.Queries;

public record GetDueCardsQuery(Guid TopicId) : IRequest<List<Domain.Entities.Card>>;

public class GetDueCardsQueryHandler(IAppContextService contextService, ICardService cardService) : IRequestHandler<GetDueCardsQuery, List<Domain.Entities.Card>>
{
    public async Task<List<Domain.Entities.Card>> Handle(GetDueCardsQuery request, CancellationToken cancellationToken)
    {
        var dueCards = await cardService.GetDueCardsAsync(request.TopicId, contextService.UserId!.Value);
        return dueCards.ToList();
    }
}