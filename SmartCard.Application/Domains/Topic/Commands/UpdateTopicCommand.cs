using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Constant;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Commands;

public record UpdateTopicCommand(UpdateTopicInput Topic) : IRequest<Guid>;

public class UpdateTopicCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTopicCommand, Guid>
{
    public async Task<Guid> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await unitOfWork.TopicRepository.GetIncludeAsync(request.Topic.Id);
        if (topic is null)
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
            var newCard = new Domain.Entities.Card
            {
                Terminology = card.Terminology,
                Definition = card.Definition,
                Description = card.Definition,
                TopicId = topicId,
                EasinessFactor = AppConstant.InitialEasinessFactor,
                CurrentInterval = AppConstant.InitialInterval,
                NextStudyDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(AppConstant.InitialInterval)),
                StartedStudying = false
            };
            await unitOfWork.CardRepository.InsertAsync(newCard, cancellationToken);
            return;
        }
            
        var existingCard = await unitOfWork.CardRepository.GetAsync(card.Id!.Value, cancellationToken);
        if (existingCard is not null)
        {
            if (card.IsDeleted)
            {
                await unitOfWork.CardRepository.DeleteAsync(existingCard.Id, cancellationToken);
                return;
            }
            existingCard.Terminology = card.Terminology;
            existingCard.Definition = card.Definition;
            unitOfWork.CardRepository.Update(existingCard);
        }
    }
}