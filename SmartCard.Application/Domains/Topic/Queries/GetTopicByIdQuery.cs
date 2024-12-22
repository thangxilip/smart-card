using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Queries;

public record GetTopicByIdQuery(Guid Id) : IRequest<GetTopicByIdOutput>;

public class GetTopicByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTopicByIdQuery, GetTopicByIdOutput>
{
    public async Task<GetTopicByIdOutput> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
    {
        var topic = await unitOfWork.TopicRepository.GetIncludeAsync(request.Id, x => x.Cards) ??
                    throw new UserFriendlyException(HttpStatusCode.NotFound, "Topic not found");

        return new GetTopicByIdOutput
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            Cards = topic.Cards.Select(x => new BaseCardDto
            {
                Id = x.Id,
                Terminology = x.Terminology,
                Definition = x.Definition
            }).ToList(),
            CanDoExercise = (await unitOfWork.CardRepository.GetStudyCardsByTopic(request.Id, cancellationToken)).Any()
        };
    }
}