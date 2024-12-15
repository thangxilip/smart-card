using SmartCard.Domain.Constant;

namespace SmartCard.Domain.Entities;

public class Card : BaseEntity<Guid>
{
    public string Terminology { get; set; }

    public string Definition { get; set; }

    public string? ImagePath { get; set; }

    public string? Description { get; set; }

    public double EasinessFactor { get; set; } = AppConstant.InitialEasinessFactor;

    public int CurrentInterval { get; set; } = AppConstant.InitialInterval;

    public DateOnly NextStudyDate { get; set; }

    public bool StartedStudying { get; set; }
    
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }
}