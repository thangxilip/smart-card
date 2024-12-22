using SmartCard.Domain.Models.Auth;
using SmartCard.Domain.Models.User;

namespace SmartCard.Domain.Interfaces;

public interface IUserService
{
    Task<UserModel> GetUserByEmailAsync(string email);

    Task<UserModel> CreateExternalUserIfNotExistAsync(UserInfo userInfo);
}