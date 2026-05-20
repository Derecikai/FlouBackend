using FlouBackend.Business.DTOs.Requests.FolderRequests;
using FlouBackend.Business.DTOs.Responses.FolderResponses;
using FlouBackend.Business.Interfaces.IRepositories;
using FlouBackend.Business.Interfaces.IServices;
using FlouBackend.Business.Mappings;

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
                if (request.ParentFolderId.Value == id) return null;

                // Parent must exist and belong to this user
                var parent = await _repo.GetByIdAsync(request.ParentFolderId.Value, userId);
                if (parent is null) return null;
                // Cycle check — walk up from proposed parent
                var check = parent;
                var depth = 1;
                while (check.ParentFolderId is not null)
                {
                    if (check.ParentFolderId == id) return null;  // cycle detected
                    if (depth >= MaxFolderDepth) return null;     // too deep
                    check = await _repo.GetByIdAsync(check.ParentFolderId.Value, userId);
                    if (check is null) break;
                    depth++;
                }
            }
        }

        request.ApplyTo(folder);
        await _repo.SaveChangesAsync();

       return folder.ToResponse();

    }

    // DELETE A FOLDER (cascade soft-delete)
    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        // Step 1 — verify folder exists and belongs to this user
        var folder = await _repo.GetByIdAsync(id, userId);
        if (folder is null) return false;

        // Step 2 — get the entire subtree of descendant folders
        var descendants = await _repo.GetSubtreeFoldersAsync(id, userId);

        // Step 3 — collect ALL folder IDs (root + every descendant)
        var allFolderIds = descendants.Select(f => f.Id).ToList();
        allFolderIds.Add(id); // don't forget the root folder itself

        // Step 4 — get all items living in any of these folders
        var items = await _repo.GetItemsByFolderIdsAsync(allFolderIds, userId);

        // Step 5 — soft-delete everything
        var now = DateTime.UtcNow;

        foreach (var descendant in descendants)
        {
            descendant.IsDeleted = true;
            descendant.DeletedAt = now;
        }

        foreach (var item in items)
        {
            item.IsDeleted = true;
            item.DeletedAt = now;
        }

        folder.IsDeleted = true;
        folder.DeletedAt = now;

        // Step 6 — one SaveChanges persists everything at once
        await _repo.SaveChangesAsync();
        return true;
    }

}
