using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Constant;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Commands;

public record UpdateTopicCommand(UpdateTopicInput Topic) : IRequest<Guid>;

public class UpdateTopicCommandHandler(IAppContextService contextService, IUnitOfWork unitOfWork, ICardService cardService) : IRequestHandler<UpdateTopicCommand, Guid>
{
    public async Task<Guid> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await unitOfWork.TopicRepository.GetIncludeAsync(request.Topic.Id);
        if (topic is null || topic.CreatedBy != contextService.UserId)
        {
            throw new UserFriendlyException(HttpStatusCode.NotFound, "Topic not found");
        }
        
        topic.Name = request.Topic.Name;
        topic.Description = request.Topic.Description;
        unitOfWork.TopicRepository.Update(topic);
        
        foreach (var card in request.Topic.Cards)
        {
            await AddOrUpdateCard(topic.Id, card, cancellationToken);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }

    private async Task AddOrUpdateCard(Guid topicId, UpdateCardDto card, CancellationToken cancellationToken)
    {
        if (card.Id is null)
        {
            await cardService.CreateCardAsync(topicId, card.Front, card.Back, cancellationToken);
            return;
        }
            
        var existingCard = await unitOfWork.FlashCardRepository.GetAsync(card.Id!.Value, cancellationToken);
        if (existingCard is not null && existingCard.CreatedBy == contextService.UserId)
        {
            if (card.IsDeleted)
            {
                await unitOfWork.FlashCardRepository.DeleteAsync(existingCard.Id, cancellationToken);
                return;
            }
            existingCard.Front = card.Front;
            existingCard.Back = card.Back;
            unitOfWork.FlashCardRepository.Update(existingCard);
        }
    }
}