using MediatR;
using SmartCard.Application.Dtos.Card;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Card.Queries;

public record GetCardsForStudyingQuery(Guid TopicId) : IRequest<List<GetCardsForStudyingOutput>>;

public class GetStudyCardsByTopicHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCardsForStudyingQuery, List<GetCardsForStudyingOutput>>
{
    public async Task<List<GetCardsForStudyingOutput>> Handle(GetCardsForStudyingQuery request, CancellationToken cancellationToken)
    {
        var cards = await unitOfWork.CardRepository.GetStudyCardsByTopic(request.TopicId, cancellationToken);

        return cards.Select(x => new GetCardsForStudyingOutput
        {
            Id = x.Id,
            Terminology = x.Terminology,
            Definition = x.Definition,
            Description = x.Description,
            ImagePath = x.ImagePath
        }).ToList();
    }
}