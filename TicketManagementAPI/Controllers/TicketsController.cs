using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketManagement.API.DTOs.Tickets;
using TicketManagement.API.Models;
using TicketManagement.API.Services.Interfaces;

namespace TicketManagement.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: api/tickets
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        

        var response = await _ticketService.GetAllAsync();

        return Ok(response);
    }

    // GET: api/tickets/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        

        var response = await _ticketService.GetByIdAsync(id);

        if (response.Status == "Error")
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    // POST: api/tickets
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketDto dto)
    {
        // Get logged-in user's ID from JWT
        var userIdClaim = User.FindFirstValue(
    ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiResponse<object>(
                "Error",
                "Invalid user identity",
                null));
        }

        var response =
            await _ticketService.CreateAsync(dto, userId);

        if (response.Status == "Error")
        {
            return BadRequest(response);
        }

        return StatusCode(
            StatusCodes.Status201Created,
            response
        );
    }

    // GET: api/tickets/my
    [HttpGet("my")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userIdClaim = User.FindFirstValue(
          ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiResponse<object>(
                "Error",
                "Invalid user identity",
                null));
        }

        var response =
            await _ticketService.GetMyTicketsAsync(userId);

        return Ok(response);
    }

    // GET: api/tickets/assigned
    [HttpGet("assigned")]
    public async Task<IActionResult> GetAssignedTickets()
    {
        var userIdClaim = User.FindFirstValue(
             ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiResponse<object>(
                "Error",
                "Invalid user identity",
                null));
        }

        var response =
            await _ticketService.GetAssignedTicketsAsync(userId);

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateTicketDto dto)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiResponse<object>(
                "Error",
                "Invalid user identity",
                null));
        }

        var role = User.FindFirstValue(
            ClaimTypes.Role);

        if (string.IsNullOrWhiteSpace(role))
        {
            return Unauthorized(new ApiResponse<object>(
                "Error",
                "User role not found",
                null));
        }

        var response = await _ticketService.UpdateAsync(
            id,
            dto,
            userId,
            role);

        if (response.Status == "Error")
        {
            if (response.Message.Contains("authorized"))
                return Forbid();

            if (response.Message == "Ticket not found")
                return NotFound(response);

            return BadRequest(response);
        }

        return Ok(response);
    }
}