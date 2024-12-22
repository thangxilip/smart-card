using SmartCard.Application.Dtos.Card;

namespace SmartCard.Application.Dtos.Topic;

public class GetTopicByIdOutput
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    
    public List<BaseCardDto> Cards { get; set; }

    public bool CanDoExercise { get; set; }
}