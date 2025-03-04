using SmartCard.Domain.Enums;

namespace SmartCard.Domain.Models.FSRS;

public class FSRSCard
{
    public double Difficulty { get; set; } = 0;  // Initial difficulty will be set by D0(G)
    public double Stability { get; set; } = 0;
    public CardState State { get; set; } = CardState.New;
    public DateTime? LastReview { get; set; }
    public DateTime? DueDate { get; set; }
    public int RepsCount { get; set; } = 0;
    public int LapseCount { get; set; } = 0;
}