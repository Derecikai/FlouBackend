namespace FlouBackend.Business.DTOs.Requests;

public class CreateItemRequest
{
    public string Name { get; set; } = null!;
    public int FolderId { get; set; }
}
