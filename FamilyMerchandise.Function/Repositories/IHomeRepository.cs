
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IHomeRepository
{
    public Task<Home> GetHome(Guid homeId);
}