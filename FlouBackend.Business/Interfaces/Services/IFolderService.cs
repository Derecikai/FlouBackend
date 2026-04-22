using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.Interfaces.Services;

public interface IFolderService
{
    Task<IEnumerable<FolderResponse>> GetAllAsync(string userId);
    Task<FolderResponse?> GetByIdAsync(int id, string userId);
    Task<FolderResponse> CreateAsync(CreateFolderRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
