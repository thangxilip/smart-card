using SmartCard.Domain.Enums;

namespace SmartCard.Domain.Models.User;

public class UserModel
{
    public Guid Id { get; set; }

    public string Email { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }
    
    public string? Address { get; set; }
}