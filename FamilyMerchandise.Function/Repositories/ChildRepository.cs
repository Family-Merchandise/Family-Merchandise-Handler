using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class ChildRepository(IConnectionFactory connectionFactory) : IChildRepository
{
    private const string ChildrenTable = "inventory.children";

    public async Task<List<Child>> GetChildrenByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {ChildrenTable} WHERE HomeId = @HomeId";
        var childEntities = await con.QueryAsync<ChildEntity>(query, new { HomeId = homeId });
        return childEntities.Select(e => e.ToChild()).ToList();
    }

    public async Task<Guid> InsertChild(Guid homeId, Child child)
    {
        var childEntity = child.ToChildEntity(homeId);
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {ChildrenTable} (Name, HomeId, IconCode, DOB, Gender, PointsEarned) VALUES (@Name, @HomeId, @IconCode, @DOB, @Gender, @PointsEarned) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }
}