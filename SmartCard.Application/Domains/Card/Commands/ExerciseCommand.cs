using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Card;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Card.Commands;

public record ExerciseCommand(List<ExerciseInput> ExerciseInputs) : IRequest;

public class ExerciseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ExerciseCommand>
{
    public async Task Handle(ExerciseCommand request, CancellationToken cancellationToken)
    {
        foreach (var exercise in request.ExerciseInputs)
        {
            var card = await unitOfWork.CardRepository.GetIncludeAsync(exercise.CardId) ??
                       throw new UserFriendlyException(HttpStatusCode.NotFound, "Card not found");
            card.EasinessFactor = CalculateEasinessFactor(card.EasinessFactor, (int)exercise.Score);
            card.CurrentInterval = CalculateInterval(card.CurrentInterval, card.EasinessFactor);
            card.NextStudyDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(card.CurrentInterval));
            if (!card.StartedStudying)
            {
                card.StartedStudying = true;
            }
            unitOfWork.CardRepository.Update(card);
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