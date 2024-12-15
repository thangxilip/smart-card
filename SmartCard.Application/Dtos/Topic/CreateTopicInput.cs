using SmartCard.Application.Dtos.Card;

namespace SmartCard.Application.Dtos.Topic;

public class CreateTopicInput
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public List<BaseCardDto> Cards { get; set; }
}