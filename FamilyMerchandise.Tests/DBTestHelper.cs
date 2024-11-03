using Dapper;
using FamilyMerchandise.Function.Entities;
using Bogus;
using Bogus.DataSets;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Repositories;

namespace FamilyMerchandise.Tests;

public class FamilyMerchandiseDbHelper(FunctionTestFixture fixture) : IClassFixture<FunctionTestFixture>
{
    private readonly Faker _faker = new();

    [Fact]
    public async Task InsertInitialData()
    {
        var homeRepo = new HomeRepository(fixture.ConnectionFactory);
        var childRepo = new ChildRepository(fixture.ConnectionFactory);
        var parentRepo = new ParentRepository(fixture.ConnectionFactory);
        var assignmentRepo = new AssignmentRepository(fixture.ConnectionFactory);
        var stepRepo = new StepRepository(fixture.ConnectionFactory);
        var wishRepo = new WishRepository(fixture.ConnectionFactory);
        var achievementRepo = new AchievementRepository(fixture.ConnectionFactory);
        var penaltyRepo = new PenaltyRepository(fixture.ConnectionFactory);

        var home = new Home()
        {
            Name = _faker.Address.FullAddress(),
        };
        var homeId = await homeRepo.InsertHome(home);

        var child = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
            PointsEarned = _faker.Random.Int(0, 9999),
        };
        var child2 = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.BOY),
            PointsEarned = _faker.Random.Int(0, 9999),
        };

        var childId = await childRepo.InsertChild(homeId, child);
        var childId2 = await childRepo.InsertChild(homeId, child2);

        var parent = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.MOTHER),
        };
        var parent2 = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.FATHER),
        };

        var parentId = await parentRepo.InsertParent(homeId, parent);
        var parentId2 = await parentRepo.InsertParent(homeId, parent2);

        var assignmentRequest1 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AssignmentName = "Assignment 1",
            AssignmentIconCode = _faker.Random.Int(0, 100),
            AssignmentDescription = _faker.Lorem.Sentence(50),
            DueDate = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentRequest2 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            AssignmentName = "Assignment 1",
            AssignmentDescription = _faker.Lorem.Sentence(50),
            AssignmentIconCode = _faker.Random.Int(0, 100),
            DueDate = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentId1 = await assignmentRepo.InsertAssignment(assignmentRequest1);
        var assignmentId2 = await assignmentRepo.InsertAssignment(assignmentRequest2);

        var stepRequest1 = new CreateStepRequest
        {
            AssignmentId = assignmentId1,
            StepOrder = 0,
            StepDescription = _faker.Lorem.Sentence(20),
        };

        var stepRequest2 = new CreateStepRequest
        {
            AssignmentId = assignmentId1,
            StepOrder = 1,
            StepDescription = _faker.Lorem.Sentence(20),
        };

        var stepRequest3 = new CreateStepRequest
        {
            AssignmentId = assignmentId2,
            StepOrder = 0,
            StepDescription = _faker.Lorem.Sentence(20),
        };
        await stepRepo.InsertStep(stepRequest1);
        await stepRepo.InsertStep(stepRequest2);
        await stepRepo.InsertStep(stepRequest3);

        var wishRequest1 = new CreateWishRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            WishName = _faker.Random.Word(),
            WishDescription = _faker.Lorem.Sentence(50),
            WishIconCode = _faker.Random.Int(0, 100),
        };
        var wishRequest2 = new CreateWishRequest()
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            WishName = _faker.Random.Word(),
            WishDescription = _faker.Lorem.Sentence(50),
            WishIconCode = _faker.Random.Int(0, 100),
        };

        await wishRepo.InsertWish(wishRequest1);
        await wishRepo.InsertWish(wishRequest2);

        var achievementRequest1 = new CreateAchievementRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AchievementName = "Achievement 1",
            AchievementDescription = _faker.Lorem.Sentence(50),
            AchievementIconCode = _faker.Random.Int(0, 100),
            AchievementPointsGranted = _faker.Random.Int(100, 9999),
        };
        var achievementRequest2 = new CreateAchievementRequest()
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            AchievementName = "Achievement 1",
            AchievementDescription = _faker.Lorem.Sentence(50),
            AchievementIconCode = _faker.Random.Int(0, 100),
            AchievementPointsGranted = _faker.Random.Int(100, 9999),
        };

        await achievementRepo.InsertAchievement(achievementRequest1);
        await achievementRepo.InsertAchievement(achievementRequest2);

        var penaltyRequest = new CreatePenaltyRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            PenaltyName = "Penalty 1",
            PenaltyReason = _faker.Lorem.Sentence(50),
            PenaltyIconCode = _faker.Random.Int(0, 100),
            PenaltyPointsDeducted = _faker.Random.Int(100, 500),
        };

        await penaltyRepo.InsertPenalty(penaltyRequest);
    }
}