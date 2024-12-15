namespace SmartCard.Domain.Entities;

public abstract class BaseEntity<T>
{
    public T Id { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}