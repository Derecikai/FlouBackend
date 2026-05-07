using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.Interfaces.IServices;

public interface IItemService
{
    Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId);
    Task<ItemResponse?> GetByIdAsync(Guid id, string userId);
    Task<IEnumerable<ItemResponse>> GetAllForUserAsync(string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
    Task<ItemResponse?> UpdateAsync(UpdateItemRequest request, Guid id, string userId);
}
