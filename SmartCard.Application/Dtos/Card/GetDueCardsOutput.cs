namespace SmartCard.Application.Dtos.Card;

public class GetDueCardsOutput : BaseCardDto
{
    public string? ImagePath { get; set; }

    public string? Description { get; set; }
}