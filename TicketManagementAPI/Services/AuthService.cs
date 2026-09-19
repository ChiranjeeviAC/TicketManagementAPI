using Microsoft.AspNetCore.Identity;
using TicketManagement.API.DTOs.Auth;
using TicketManagement.API.Models;
using TicketManagement.API.Models.Entities;
using TicketManagement.API.Models.Enums;
using TicketManagement.API.Repositories.Interfaces;
using TicketManagement.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
namespace TicketManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    private readonly IConfiguration _configuration;

    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<ApiResponse<object>> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return new ApiResponse<object>(
                "Error",
                "Full name is required",
                null);

        if (string.IsNullOrWhiteSpace(dto.Email))
            return new ApiResponse<object>(
                "Error",
                "Email is required",
                null);

        if (string.IsNullOrWhiteSpace(dto.Password))
            return new ApiResponse<object>(
                "Error",
                "Password is required",
                null);

        var existingUser = await _authRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return new ApiResponse<object>(
                "Error",
                "Email already registered",
                null);
        }

        if (!Enum.TryParse<UserRole>(
    dto.Role,
    true,
    out var role))
        {
            return new ApiResponse<object>(
                "Error",
                "Role must be Employee or Solver",
                null);
        }

        if (role == UserRole.Admin)
        {
            return new ApiResponse<object>(
                "Error",
                "Admin registration is not allowed",
                null);
        }

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var hasher = new PasswordHasher<User>();

        user.PasswordHash = hasher.HashPassword(
            user,
            dto.Password);

        await _authRepository.AddUserAsync(user);

        return new ApiResponse<object>(
            "Success",
            "User registered successfully",
            new
            {
                user.Id,
                user.FullName,
                user.Email,
                Role = user.Role.ToString()
            });
    }

    public async Task<ApiResponse<object>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return new ApiResponse<object>(
                "Error",
                "Email is required",
                null);
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            return new ApiResponse<object>(
                "Error",
                "Password is required",
                null);
        }

        var user = await _authRepository.GetByEmailAsync(
            dto.Email.Trim().ToLower());

        if (user == null)
        {
            return new ApiResponse<object>(
                "Error",
                "Invalid email or password",
                null);
        }

        if (!user.IsActive)
        {
            return new ApiResponse<object>(
                "Error",
                "User account is inactive",
                null);
        }

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return new ApiResponse<object>(
                "Error",
                "Invalid email or password",
                null);
        }

        var token = GenerateJwtToken(user);

        return new ApiResponse<object>(
            "Success",
            "Login successful",
            new
            {
                token,
                userId = user.Id,
                fullName = user.FullName,
                email = user.Email,
                role = user.Role.ToString()
            });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("JWT key is not configured.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString()),

        new Claim(
            ClaimTypes.Name,
            user.FullName),

        new Claim(
            ClaimTypes.Email,
            user.Email),

        new Claim(
            ClaimTypes.Role,
            user.Role.ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}