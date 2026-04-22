using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Business.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace FlouBackend.Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new AuthResponse { Success = false, Errors = result.Errors.Select(e => e.Description) };

        return new AuthResponse { Success = true, Token = GenerateTokenPlaceholder(user) };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new AuthResponse { Success = false, Errors = new[] { "Invalid credentials." } };

        var check = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!check.Succeeded)
            return new AuthResponse { Success = false, Errors = new[] { "Invalid credentials." } };

        return new AuthResponse { Success = true, Token = GenerateTokenPlaceholder(user) };
    }

    private static string GenerateTokenPlaceholder(IdentityUser user) =>
        $"TODO-JWT-FOR-{user.Id}";
}
