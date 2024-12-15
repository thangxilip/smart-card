using SmartCard.Application.Dtos.Topic;

namespace SmartCard.Application.Dtos.Card;

public class GetCardsForStudyingOutput : BaseCardDto
{
    public string? ImagePath { get; set; }

    public string? Description { get; set; }
}