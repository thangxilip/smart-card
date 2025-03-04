using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Domain.Constant;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Commands;

public record CreateTopicCommand(
    string Name,
    string? Description,
    List<BaseCardDto> Cards) : IRequest<Guid>;

public class CreateTopicCommandHandler(IUnitOfWork unitOfWork, IAppContextService contextService, ICardService cardService) : IRequestHandler<CreateTopicCommand, Guid>
{
    public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topicExisting = await unitOfWork.TopicRepository.AnyAsync(x =>
            x.CreatedBy == contextService.UserId &&
            x.Name.ToLower() == request.Name.Trim().ToLower(),
            cancellationToken);
        if (topicExisting)
        {
            throw new UserFriendlyException(HttpStatusCode.BadRequest, "Topic with the same name already exists");
        }
        
        var topic = new Domain.Entities.Topic
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedBy = contextService.UserId,
            CreatedAt = DateTime.UtcNow,
            Cards = new List<Domain.Entities.Card>()
        };
        await unitOfWork.TopicRepository.InsertAsync(topic, cancellationToken);
        
        foreach (var card in request.Cards)
        {
            await cardService.CreateCardAsync(topic.Id, card.Front, card.Back, cancellationToken);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }
}