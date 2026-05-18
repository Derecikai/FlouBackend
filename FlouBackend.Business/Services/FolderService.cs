using FlouBackend.Business.DTOs.Requests.FolderRequests;
using FlouBackend.Business.DTOs.Responses.FolderResponses;
using FlouBackend.Business.Interfaces.IRepositories;
using FlouBackend.Business.Interfaces.IServices;
using FlouBackend.Business.Mappings;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Services;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repo;
    private const int MaxFolderDepth = 10;

    public FolderService(IFolderRepository repo) => _repo = repo;


    //

    // CREATE A FOLDER
    public async Task<FolderResponse?> CreateAsync(CreateFolderRequest request, string userId)
    {
        // Step 1 — if placing inside a parent, validate it exists and belongs to this user
        if (request.ParentFolderId is not null)     
        {
            var parent = await _repo.GetByIdAsync(request.ParentFolderId.Value, userId);
            if (parent is null) return null;    // parent not found or belongs to another user

            // Step 2 — enforce depth cap: walk up the tree and count levels
            var depth = 1;
            var current = parent;
            while (current.ParentFolderId is not null)
            {
                if (depth >= MaxFolderDepth) return null;   // too deep
                current = await _repo.GetByIdAsync(current.ParentFolderId.Value, userId);
                if (current is null) break;
                depth++;
            }
        }

        // Step 3 — create the entity and persist
        var folder = request.ToEntity(userId);
        await _repo.AddAsync(folder);
        await _repo.SaveChangesAsync();

        return folder.ToResponse();
    }

    public async Task<FolderContentsResponse?> GetByIdAsync(Guid id, string userId)
    {

        var folder = await _repo.GetByIdAsync(id, userId);

        if (folder is null)
        {
            return null;   //    We don't have actually the correct folder for the correct user.
        }

        //HERE WE FETCH ALL THE FOLDERS CHILD FOLDERS
        var subFolders = await _repo.GetChildFoldersAsync(id, userId);

        //HERE WE FETCH ALL THE CHILD ITEMS OF THE FOLDER
        var subItems = await _repo.GetItemsInFolderAsync(id, userId);

        //HERE WE RETURN THE RESPONSE OF TYPE FolderContentsResponse
        return new FolderContentsResponse
        {
            Folder = folder.ToResponse(),
            Subfolders = subFolders.Select(f => f.ToResponse()).ToList(),
            Items = subItems.Select(i => i.ToResponse()).ToList()
        };

    }


    // GET ROOT FOLDERS
    public async Task<IEnumerable<FolderResponse>> GetRootFoldersAsync(string userId)
    {
        var folders = await _repo.GetRootFoldersAsync(userId);
        return folders.Select(f => f.ToResponse());
    }


    public async Task<FolderResponse?> UpdateAsync(UpdateFolderRequest request, Guid id, string userId)
    {
        var folder = await _repo.GetByIdAsync(id, userId);

        if (folder is null)
        {
            return null;
        }

        // Only run move validation if ParentFolderId is actually changing
        if (request.ParentFolderId != folder.ParentFolderId)
        {
            if (request.ParentFolderId is not null)
            {
                // Self-parent guard
                if (request.ParentFolderId == id) return null;

                // Parent must exist and belong to this user
                var parent = await _repo.GetByIdAsync(request.ParentFolderId.Value, userId);
                if (parent is null) return null;

                // Cycle check — new parent can't be a descendant of this folder
                // (to be implemented with GetSubtreeAsync in step 4)

                // Depth cap — same logic as CreateAsync
            }
        }

        request.ApplyTo(folder);
        await _repo.SaveChangesAsync();

       return folder.ToResponse();

    }

    public Task<bool> DeleteAsync(Guid id, string userId) =>
        throw new NotImplementedException();
}
