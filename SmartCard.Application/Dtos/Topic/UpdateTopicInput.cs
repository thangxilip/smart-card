using SmartCard.Application.Dtos.Card;

namespace SmartCard.Application.Dtos.Topic;

public class UpdateTopicInput
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    
    public List<UpdateCardDto> Cards { get; set; }
}