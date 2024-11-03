using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Services;

public interface IChildService
{
    // Profile
    public Task<Child> GetProfileByChildId(Guid childId);
    
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId);
    
    // Wishes
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId);
    public Task<Guid> CreateWish(CreateWishRequest request);
    public Task EditWish(Guid wishId);
    
    // Achievements
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId);
    
    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId);
}