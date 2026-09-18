using Microsoft.AspNetCore.Mvc;
using TicketManagement.API.DTOs.Auth;
using TicketManagement.API.Services.Interfaces;

namespace TicketManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var response = await _authService.RegisterAsync(dto);

        if (response.Status == "Error")
            return BadRequest(response);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }
}