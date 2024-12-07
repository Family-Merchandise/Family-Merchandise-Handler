using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class AppUserService(
    IAppUserRepository appUserRepository,
    IHomeRepository homeRepository,
    ILogger<HomeService> logger)
    : IAppUserService
{
    public async Task<Guid> RegisterUser(AppUser user)
    {
        logger.LogInformation($"Registering a user to the app with email: {user.Email}");
        var userId = await appUserRepository.InsertIfNotExist(user);
        logger.LogInformation($"Successfully registered user with Id: {userId}");
        return userId;
    }

    public async Task<Guid> GetHomeIdByAppUserId(Guid userId)
    {
        logger.LogInformation("Getting home ids by app user Id");
        var homeId = await homeRepository.GetHomeIdByAppUserId(userId);
        logger.LogInformation($"Successfully get home : {homeId} by app user Id: {userId}");
        return homeId;
    }
}