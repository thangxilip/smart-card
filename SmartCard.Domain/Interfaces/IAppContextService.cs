namespace SmartCard.Domain.Interfaces;

public interface IAppContextService
{
    public Guid? UserId { get; }
    
    public string Email { get; }
}