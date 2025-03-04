namespace SmartCard.Application.Dtos.Topic;

public class GetTopicStatisticsOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public int TotalCards { get; set; }
    public int NewCards { get; set; }
    public int LearningCards { get; set; }
    public int ReviewCards { get; set; }
    public int DueCards { get; set; }
    public int TotalReviews { get; set; }
    public double AverageStability { get; set; }

    public string? Avatar { get; set; }
}