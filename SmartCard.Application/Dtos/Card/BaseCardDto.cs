namespace SmartCard.Application.Dtos.Card;

public class BaseCardDto
{
    public Guid? Id { get; set; }
    
    public string? Key { get; set; } // used for list item on FE

    public string Front { get; set; }
    
    public string Back { get; set; }
}