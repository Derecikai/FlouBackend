namespace FlouBackend.Business.DTOs.Responses;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}
