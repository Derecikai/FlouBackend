using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;

namespace FlouBackend.Business.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
