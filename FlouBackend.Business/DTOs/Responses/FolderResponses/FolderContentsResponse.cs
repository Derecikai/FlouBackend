using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.DTOs.Responses.FolderResponses;

public class FolderContentsResponse
{
    public FolderResponse Folder { get; set; } = null!;
    public List<FolderResponse> Subfolders { get; set; } = new();
    public List<ItemResponse> Items { get; set; } = new();
}
