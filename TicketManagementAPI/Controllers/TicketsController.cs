using Microsoft.AspNetCore.Mvc;
using TicketManagement.API.DTOs.Tickets;
using TicketManagement.API.Helpers;
using TicketManagement.API.Models;
using TicketManagement.API.Services.Interfaces;

namespace TicketManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        if (!TokenHelper.HasToken(Request))
        {
            return Unauthorized(
                new ApiResponse<object>(
                    "Error",
                    "No token available",
                    null
                )
            );
        }

        var response = await _ticketService.GetAllAsync();

        return Ok(response);
    }

    // GET: api/tickets/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!TokenHelper.HasToken(Request))
        {
            return Unauthorized(
                new ApiResponse<object>(
                    "Error",
                    "No token available",
                    null
                )
            );
        }

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
        if (!TokenHelper.HasToken(Request))
        {
            return Unauthorized(
                new ApiResponse<object>(
                    "Error",
                    "No token available",
                    null
                )
            );
        }

        // TEMPORARY USER ID FOR TESTING
        // This will be replaced with JWT UserId.
        int userId = 1;

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
        if (!TokenHelper.HasToken(Request))
        {
            return Unauthorized(
                new ApiResponse<object>(
                    "Error",
                    "No token available",
                    null
                )
            );
        }

        // TEMPORARY USER ID FOR TESTING
        int userId = 1;

        var response =
            await _ticketService.GetMyTicketsAsync(userId);

        return Ok(response);
    }

    // GET: api/tickets/assigned
    [HttpGet("assigned")]
    public async Task<IActionResult> GetAssignedTickets()
    {
        if (!TokenHelper.HasToken(Request))
        {
            return Unauthorized(
                new ApiResponse<object>(
                    "Error",
                    "No token available",
                    null
                )
            );
        }

        // TEMPORARY USER ID FOR TESTING
        int userId = 1;

        var response =
            await _ticketService.GetAssignedTicketsAsync(userId);

        return Ok(response);
    }
}