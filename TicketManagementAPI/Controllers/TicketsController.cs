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

    [HttpPut("{id:int}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignSolver(
    int id,
    [FromBody] AssignTicketDto dto)
    {
        var response = await _ticketService.AssignSolverAsync(
            id,
            dto.SolverId);

        if (response.Status == "Error")
        {
            if (response.Message == "Ticket not found" ||
                response.Message == "Solver not found")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Solver,Admin")]
    public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] UpdateTicketStatusDto dto)
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

        var response = await _ticketService.UpdateStatusAsync(
            id,
            dto,
            userId,
            role);

        if (response.Status == "Error")
        {
            if (response.Message == "Ticket not found")
                return NotFound(response);

            if (response.Message.Contains("authorized"))
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> AddComment(
    int id,
    [FromBody] AddCommentDto dto)
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

        var response = await _ticketService.AddCommentAsync(
            id,
            dto,
            userId);

        if (response.Status == "Error")
        {
            if (response.Message == "Ticket not found")
                return NotFound(response);

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("{id:int}/activity")]
    public async Task<IActionResult> GetActivity(int id)
    {
        var response =
            await _ticketService.GetActivityAsync(id);

        if (response.Status == "Error")
        {
            if (response.Message == "Ticket not found")
                return NotFound(response);

            return BadRequest(response);
        }

        return Ok(response);
    }
}