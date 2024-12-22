using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Card.Commands;

public record ScoreCardsCommand(
    Guid TopicId,
    List<ScoreInput> Cards) : IRequest;

public class ScoreCardsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ScoreCardsCommand>
{
    public async Task Handle(ScoreCardsCommand request, CancellationToken cancellationToken)
    {
        foreach (var card in request.Cards)
        {
            var existingCard = await unitOfWork.CardRepository.GetAsync(x => x.TopicId == request.TopicId && x.Id == card.Id, cancellationToken) ??
                       throw new UserFriendlyException(HttpStatusCode.BadRequest, "Card not found");
            existingCard.EasinessFactor = CalculateEasinessFactor(existingCard.EasinessFactor, (int)card.Score);
            existingCard.CurrentInterval = CalculateInterval(existingCard.CurrentInterval, existingCard.EasinessFactor);
            existingCard.NextStudyDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(existingCard.CurrentInterval));
            if (!existingCard.StartedStudying)
            {
                existingCard.StartedStudying = true;
            }
            unitOfWork.CardRepository.Update(existingCard);
        }
        
        if (request.Cards.Any())
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
    
    private static int CalculateInterval(int currentInterval, double easinessFactor)
    {
        var newInterval = currentInterval * easinessFactor;
        return (int)Math.Round(newInterval);
    }

    private static double CalculateEasinessFactor(double currentEasinessFactor, int score)
    {
        return currentEasinessFactor - 0.8 + (0.28 * score) - (0.02 * score * score);
    }
}