using FlouBackend.Business.DTOs.Requests.FolderRequests;
using FlouBackend.Business.DTOs.Responses.FolderResponses;

namespace FlouBackend.Business.Interfaces.IServices;

public interface IFolderService
{
    Task<FolderResponse?> CreateAsync(CreateFolderRequest request, string userId);
    Task<FolderContentsResponse?> GetByIdAsync(Guid id, string userId);
    Task<IEnumerable<FolderResponse>> GetRootFoldersAsync(string userId);
    Task<FolderResponse?> UpdateAsync(UpdateFolderRequest request, Guid id, string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
}
