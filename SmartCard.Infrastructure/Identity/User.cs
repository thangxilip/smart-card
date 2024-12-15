using Microsoft.AspNetCore.Identity;
using SmartCard.Domain.Enums;

namespace SmartCard.Infrastructure.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    
    public string? Address { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}