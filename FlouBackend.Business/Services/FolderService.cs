using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Business.Interfaces.Repositories;
using FlouBackend.Business.Interfaces.Services;
using FlouBackend.Business.Mappings;

namespace FlouBackend.Business.Services;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repo;

    public FolderService(IFolderRepository repo) => _repo = repo;

    public async Task<IEnumerable<FolderResponse>> GetAllAsync(string userId)
    {
        var folders = await _repo.GetAllForUserAsync(userId);
        return folders.Select(f => f.ToResponse());
    }

    public async Task<FolderResponse?> GetByIdAsync(int id, string userId)
    {
        var folder = await _repo.GetByIdAsync(id, userId);
        return folder?.ToResponse();
    }

    public async Task<FolderResponse> CreateAsync(CreateFolderRequest request, string userId)
    {
        var folder = request.ToEntity(userId);
        await _repo.AddAsync(folder);
        await _repo.SaveChangesAsync();
        return folder.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var folder = await _repo.GetByIdAsync(id, userId);
        if (folder is null) return false;

        await _repo.DeleteAsync(folder);
        await _repo.SaveChangesAsync();
        return true;
    }
}
