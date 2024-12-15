using MediatR;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Queries;

public record GetAllTopicQuery : IRequest<List<GetAllTopicOutput>>;

public class GetAllTopicQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTopicQuery, List<GetAllTopicOutput>>
{
    public async Task<List<GetAllTopicOutput>> Handle(GetAllTopicQuery request, CancellationToken cancellationToken)
    {
        var topics = (await unitOfWork.TopicRepository.GetAllAsync())
            .Select(x => new GetAllTopicOutput
            {
                Id = x.Id,
                Name = x.Name,
                NumberOfCards = x.Cards.Count,
                Avatar = null,
                Author = "Thang Vo"
            });
        return topics.ToList();
    }
}