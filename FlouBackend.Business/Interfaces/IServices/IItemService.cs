using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.Interfaces.IServices;

public interface IItemService
{
    Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId);
    Task<ItemResponse?> GetByIdAsync(Guid id, string userId);
    Task<IEnumerable<ItemResponse>> GetAllForUserAsync(string userId);
}
