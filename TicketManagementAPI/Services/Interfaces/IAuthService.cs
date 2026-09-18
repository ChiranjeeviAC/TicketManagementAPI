using TicketManagement.API.DTOs.Auth;
using TicketManagement.API.Models;

namespace TicketManagement.API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<object>> RegisterAsync(RegisterDto dto);
}