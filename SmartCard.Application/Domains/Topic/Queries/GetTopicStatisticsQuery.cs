using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Enums;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Topic.Queries;

public record GetTopicStatisticsQuery : IRequest<List<GetTopicStatisticsOutput>>;

public class GetTopicStatisticsQueryHandler(IUnitOfWork unitOfWork, IAppContextService contextService) : IRequestHandler<GetTopicStatisticsQuery, List<GetTopicStatisticsOutput>>
{
    public async Task<List<GetTopicStatisticsOutput>> Handle(GetTopicStatisticsQuery request, CancellationToken cancellationToken)
    {
        var topics = (await unitOfWork.TopicRepository.GetAllAsync(x => x.CreatedBy == contextService.UserId))
            .Select(x => new 
            {
                x.Id,
                x.Name,
                x.Cards,
            });

        if (!topics.Any())
        {
            return [];
        }
        
        
        var result = topics.Select(topic => new GetTopicStatisticsOutput
        {
            Id = topic.Id,
            Name = topic.Name,
            TotalCards = topic.Cards!.Count,
            NewCards = topic.Cards.Count(c => c.State.Status == CardState.New),
            LearningCards = topic.Cards.Count(c => c.State.Status == CardState.Learning),
            ReviewCards = topic.Cards.Count(c => c.State.Status == CardState.Review),
            DueCards = topic.Cards.Count(c => c.State.NextReviewAt <= DateTime.UtcNow),
            // TotalReviews = await _context.Reviews
            //     .CountAsync(r => r.Card.DeckId == deckId),
            // AverageStability = await _context.CardStates
            //     .Where(s => s.Card.DeckId == deckId && s.Status == "REVIEW")
            //     .AverageAsync(s => s.Stability)
        });
        return result.ToList();
    }
}