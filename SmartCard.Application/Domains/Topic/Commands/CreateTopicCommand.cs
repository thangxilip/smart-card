using MediatR;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Constant;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Commands;

public record CreateTopicCommand(
    string Name,
    string? Description,
    List<BaseCardDto> Cards) : IRequest<Guid>;

public class CreateTopicCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTopicCommand, Guid>
{
    public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Domain.Entities.Topic
        {
            Name = request.Name,
            Description = request.Description,
            Cards = new List<Domain.Entities.Card>()
        };
        await unitOfWork.TopicRepository.InsertAsync(topic, cancellationToken);

        foreach (var card in request.Cards)
        {
            var cardEntity = new Domain.Entities.Card
            {
                Terminology = card.Terminology,
                Definition = card.Definition,
                Description = card.Definition,
                TopicId = topic.Id,
                EasinessFactor = AppConstant.InitialEasinessFactor,
                CurrentInterval = AppConstant.InitialInterval,
                NextStudyDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(AppConstant.InitialInterval)),
                StartedStudying = false
            };
            topic.Cards.Add(cardEntity);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }
}