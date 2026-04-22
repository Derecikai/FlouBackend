namespace FlouBackend.Business.DTOs.Responses;

public class FolderResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
}
