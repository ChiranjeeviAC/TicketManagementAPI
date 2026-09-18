using Microsoft.AspNetCore.Identity;
using TicketManagement.API.DTOs.Auth;
using TicketManagement.API.Models;
using TicketManagement.API.Models.Entities;
using TicketManagement.API.Models.Enums;
using TicketManagement.API.Repositories.Interfaces;
using TicketManagement.API.Services.Interfaces;

namespace TicketManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
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

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Role = UserRole.Employee,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Password hashing
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
}