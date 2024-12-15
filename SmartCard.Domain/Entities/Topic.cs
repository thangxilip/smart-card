namespace SmartCard.Domain.Entities;

public class Topic : BaseEntity<Guid>
{
    public string Name { get; set; }

    public string? Description { get; set; }
    
    public ICollection<Card> Cards { get; set; }
}