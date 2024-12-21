using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AchievementRepository(IConnectionFactory connectionFactory) : IAchievementRepository
{
    private const string AchievementsTable = "inventory.achievements";
    public const string ChildrenTable = "inventory.children";
    public const string ParentTable = "inventory.parents";

    public async Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AchievementsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                 LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                 WHERE a.HomeId = @HomeId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { HomeId = homeId });
        return achievements.ToList();
    }

    public async Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AchievementsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                 LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                 WHERE a.VisionaryId = @VisionaryId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { VisionaryId = parentId });
        return achievements.ToList();
    }

    public async Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AchievementsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                 LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                 WHERE a.AchieverId = @AchieverId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { AchieverId = childId });
        return achievements.ToList();
    }

    private readonly Func<AchievementEntity, ChildEntity, ParentEntity, Achievement> _mapEntitiesToAchievementModel =
        (a, c, p) =>
        {
            var achievement = a.ToAchievement();
            achievement.Achiever = c.ToChild();
            achievement.Visionary = p.ToParent();
            return achievement;
        };


    public async Task<Guid> InsertAchievement(CreateAchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {AchievementsTable} (Name, HomeId, IconCode, PointsGranted, Description, VisionaryId, AchieverId) VALUES (@Name, @HomeId, @IconCode, @PointsGranted, @Description, @VisionaryId, @AchieverId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }

    public async Task<Guid> EditAchievementByAchievementId(EditAchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                UPDATE {AchievementsTable} 
                SET Name = @Name,
                 IconCode = @IconCode,
                 Description = @Description,
                 PointsGranted = @PointsGranted,
                 VisionaryId = @VisionaryId,
                 AchieverId = @AchieverId
                WHERE Id = @Id
                RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }

    public async Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId,
        bool isAchievementGranted)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"UPDATE {AchievementsTable} SET AchievedDateUtc = @AchievedDateUtc WHERE Id = @Id RETURNING Id, AchieverId AS ChildId, PointsGranted AS Points;";
        return await con.QuerySingleAsync<EditAchievementEntityResponse>(query,
            new { Id = achievementId, AchievedDateUtc = isAchievementGranted ? DateTime.UtcNow : (DateTime?)null });
    }

    public async Task DeleteAchievementByAchievementId(Guid achievementId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query = $"DELETE FROM {AchievementsTable} where id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = achievementId });
    }
}