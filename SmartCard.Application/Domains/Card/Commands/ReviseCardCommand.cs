using System.Net;
using MediatR;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Domain.Enums;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Repositories.Base;

namespace SmartCard.Application.Domains.Card.Commands;

public record ReviseCardCommand(Guid CardId, CardRating Rating) : IRequest<bool>;

public class ReviseCardCommandHandler(IAppContextService contextService, IUnitOfWork unitOfWork, ICardService cardService) : IRequestHandler<ReviseCardCommand, bool>
{
    public async Task<bool> Handle(ReviseCardCommand request, CancellationToken cancellationToken)
    {
        var card = await unitOfWork.FlashCardRepository.GetAsync(request.CardId, cancellationToken);
        if (card == null || card.CreatedBy != contextService.UserId)
        {
            throw new UserFriendlyException(HttpStatusCode.NotFound, "Card not found");
        }
        
        await cardService.ReviewCardAsync(request.CardId, request.Rating, cancellationToken);
        return true;
    }
}