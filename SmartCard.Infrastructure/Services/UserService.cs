using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartCard.Application.Common.Exceptions;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.Auth;
using SmartCard.Domain.Models.User;
using SmartCard.Infrastructure.Identity;

namespace SmartCard.Infrastructure.Services;

public class UserService(UserManager<User> userManager, IMapper mapper) : IUserService
{
    public async Task<UserModel> GetUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new UserFriendlyException(HttpStatusCode.NotFound, "User not found");
        }

        return mapper.Map<UserModel>(user);
    }
    
    public async Task<UserModel> CreateExternalUserIfNotExistAsync(UserInfo userInfo)
    {
        var existingUser = await userManager.FindByEmailAsync(userInfo.Email);
        if (existingUser != null)
        {
            return mapper.Map<UserModel>(existingUser);;
        }
        
        var user = new User
        {
            UserName = userInfo.Email,
            Email = userInfo.Email,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return mapper.Map<UserModel>(user);;
    }
}