namespace SmartCard.Application.Dtos.Topic;

public class GetAllTopicOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public string? Avatar { get; set; }
}