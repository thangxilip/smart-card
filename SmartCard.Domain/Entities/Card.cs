using System.ComponentModel.DataAnnotations.Schema;
using SmartCard.Domain.Enums;

namespace SmartCard.Domain.Entities;

public class Card : BaseEntity<Guid>
{
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }
    
    public string Front { get; set; }
    public string Back { get; set; }

    public Guid StateId { get; set; }
    public FlashCardState State { get; set; }
}

public class FlashCardState : BaseEntity<Guid>
{
    public Guid CardId { get; set; }
    public Card Card { get; set; }

    public double Difficulty { get; set; }
    public double Stability { get; set; }
    public DateTime? LastReviewAt { get; set; }
    public DateTime? NextReviewAt { get; set; }
    public int ReviewCount { get; set; }
    public int LapseCount { get; set; }
    public CardState Status { get; set; }
}

public class ReviewHistory : BaseEntity<Guid>
{
    public Guid CardId { get; set; }
    public Card Card { get; set; }

    public CardRating Rating { get; set; }
    public DateTime ReviewAt { get; set; }
    public int TimeTaken { get; set; } // in miliseconds
    public double ScheduledDays { get; set; } // How many days were scheduled
    public double ElapsedDays { get; set; } // Actual days elapsed since last review
    public double PreviousDifficulty { get; set; }
    public double PreviousStability { get; set; }
    public double NewDifficulty { get; set; }
    public double NewStability { get; set; }
}