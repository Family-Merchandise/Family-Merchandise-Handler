using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IWishRepository
{
    public Task<Guid> InsertWish(CreateWishRequest request);
}