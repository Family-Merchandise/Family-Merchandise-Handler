using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IAppUserService
{
    public Task<Guid> RegisterUser(AppUser appUser);
}