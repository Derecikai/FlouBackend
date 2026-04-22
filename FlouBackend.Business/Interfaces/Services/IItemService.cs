using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.Interfaces.Services;

public interface IItemService
{
    Task<IEnumerable<ItemResponse>> GetByFolderAsync(int folderId, string userId);
    Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
