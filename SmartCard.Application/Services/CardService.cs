using SmartCard.Domain.Entities;
using SmartCard.Domain.Enums;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.FSRS;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Services;

public class CardService(IAppContextService contextService, IUnitOfWork unitOfWork, IFSRS5Scheduler scheduler) : ICardService
{
    public async Task<Card> CreateCardAsync(Guid topicId, string front, string back,
        CancellationToken cancellationToken)
    {
        var card = new Card
        {
            Id = Guid.NewGuid(),
            TopicId = topicId,
            Front = front,
            Back = back,
            CreatedAt = DateTime.Now,
            CreatedBy = contextService.UserId
        };

        // Create initial card state
        var cardState = new FlashCardState
        {
            Card = card,
            Status = CardState.New,
            CreatedAt = DateTime.Now,
            CreatedBy = contextService.UserId
        };

        await unitOfWork.FlashCardRepository.InsertAsync(card, cancellationToken);
        await unitOfWork.FlashCardStateRepository.InsertAsync(cardState, cancellationToken);
        return card;
    }
    
    public async Task<IEnumerable<Card>> GetDueCardsAsync(Guid topicId, Guid userId, int limit = 20)
    {
        var now = DateTime.UtcNow;
        var dueCards = unitOfWork.FlashCardRepository
            .GetAllIncluding(x => x.State)
            .Where(x => x.TopicId == topicId && x.CreatedBy == userId &&
                        (x.State.Status == CardState.New || x.State.NextReviewAt <= now))
            .OrderBy(c => c.State.Status == CardState.New ? 0 : 1)
            .ThenBy(c => c.State.NextReviewAt)
            .Take(limit)
            .ToList();
                

        return await Task.FromResult(dueCards);
    }

    public async Task<Card> ReviewCardAsync(Guid cardId, CardRating rating, CancellationToken cancellationToken)
    {
        // Get card with its state and verify ownership
        var card = await unitOfWork.FlashCardRepository.GetIncludeAsync(
            x => x.Id == cardId && x.Topic.CreatedBy == contextService.UserId,
            x => x.State, 
            x => x.Topic);

        if (card == null)
        {
            throw new UnauthorizedAccessException("Card not found or access denied");
        }

        var now = DateTime.UtcNow;
        var sameDayReview = card.State.LastReviewAt?.Date == now.Date;

        // Create FSRS Card object from database state
        var fsrsCard = new FSRSCard
        {
            Difficulty = card.State.Difficulty,
            Stability = card.State.Stability,
            State = card.State.Status,
            LastReview = card.State.LastReviewAt,
            DueDate = card.State.NextReviewAt,
            RepsCount = card.State.ReviewCount,
            LapseCount = card.State.LapseCount
        };

        // Apply FSRS algorithm
        var (updatedCard, reviewLog) = scheduler.Review(fsrsCard, rating, now, sameDayReview);

        // Create review history
        var review = new ReviewHistory
        {
            CardId = cardId,
            Rating = rating,
            ReviewAt = now,
            TimeTaken = 0, // You might want to track this from the UI
            ScheduledDays = reviewLog.ScheduledDays,
            ElapsedDays = reviewLog.ElapsedDays,
            PreviousDifficulty = reviewLog.PreviousDifficulty,
            PreviousStability = reviewLog.PreviousStability,
            NewDifficulty = updatedCard.Difficulty,
            NewStability = updatedCard.Stability,
            CreatedAt = DateTime.Now,
            CreatedBy = contextService.UserId
        };

        // Update card state
        card.State.Difficulty = updatedCard.Difficulty;
        card.State.Stability = updatedCard.Stability;
        card.State.LastReviewAt = updatedCard.LastReview;
        card.State.NextReviewAt = updatedCard.DueDate;
        card.State.Status = updatedCard.State;
        card.State.ReviewCount = updatedCard.RepsCount;
        card.State.LapseCount = updatedCard.LapseCount;

        unitOfWork.FlashCardStateRepository.Update(card.State);
        await unitOfWork.ReviewHistoryRepository.InsertAsync(review, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return card;
    }
}