using AutoMapper;
using SmartCard.Domain.Models.User;
using SmartCard.Infrastructure.Identity;

namespace SmartCard.Infrastructure.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>();
    }
}