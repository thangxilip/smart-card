using SmartCard.Domain.Enums;

namespace SmartCard.Domain.Models.FSRS;

public class ReviewLog
{
    public DateTime ReviewTime { get; set; }
    public CardRating Rating { get; set; }
    public double ElapsedDays { get; set; }
    public double ScheduledDays { get; set; }
    public CardState State { get; set; }
    public double PreviousDifficulty { get; set; }
    public double PreviousStability { get; set; }
    public double Retrievability { get; set; }
}