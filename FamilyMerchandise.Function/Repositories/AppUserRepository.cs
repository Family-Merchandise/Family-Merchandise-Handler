using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AppUserRepository(IConnectionFactory connectionFactory) : IAppUserRepository
{
    private const string AppUsersTable = "appusers";

    public async Task<AppUser> GetAppUserById(Guid appUserId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {AppUsersTable} WHERE Id = @Id";
        var appUser = await con.QuerySingleAsync<AppUserEntity>(query, new { Id = appUserId });
        return appUser.ToAppUser();
    }

    public async Task<Guid> InsertIfNotExist(AppUser user)
    {
        var appUserEntity = user.ToAppUserEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        // This will be called multiple time, we could just check if Idp id exist, because Idp Id should be Unique. Id is internal to this app only
        // TODO: Maybe Email will change in the future
        var query =
            $"INSERT INTO {AppUsersTable} (Id, Email, IdentityProvider, IdpId, Sku) VALUES (@Id, @Email, @IdentityProvider, @IdpId, @Sku) ON CONFLICT (IdpId) DO UPDATE SET IdpId = @IdpId RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, appUserEntity);
    }
}