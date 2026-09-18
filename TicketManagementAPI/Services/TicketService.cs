using TicketManagement.API.DTOs.Tickets;
using TicketManagement.API.Models;
using TicketManagement.API.Models.Entities;
using TicketManagement.API.Models.Enums;
using TicketManagement.API.Repositories.Interfaces;
using TicketManagement.API.Services.Interfaces;

namespace TicketManagement.API.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<ApiResponse<List<TicketResponseDto>>> GetAllAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();

        var data = tickets.Select(MapToDto).ToList();

        return new ApiResponse<List<TicketResponseDto>>(
            "Success",
            "Tickets retrieved successfully",
            data
        );
    }

    public async Task<ApiResponse<TicketResponseDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Invalid ticket ID",
                null
            );
        }

        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Ticket not found",
                null
            );
        }

        var data = MapToDto(ticket);

        return new ApiResponse<TicketResponseDto>(
            "Success",
            "Ticket retrieved successfully",
            data
        );
    }

    public async Task<ApiResponse<TicketResponseDto>> CreateAsync(
        CreateTicketDto dto,
        int userId)
    {
        if (userId <= 0)
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Invalid user",
                null
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Ticket title is required",
                null
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Ticket description is required",
                null
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Priority))
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Ticket priority is required",
                null
            );
        }

        if (!Enum.TryParse<TicketPriority>(
                dto.Priority,
                true,
                out var priority))
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Invalid ticket priority",
                null
            );
        }

        var ticket = new Ticket
        {
            TicketNumber = GenerateTicketNumber(),
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Priority = priority,
            Status = TicketStatus.Open,
            CreatedByUserId = userId,
            AssignedToUserId = null,
            CreatedAt = DateTime.UtcNow
        };

        await _ticketRepository.AddAsync(ticket);

        var createdTicket =
            await _ticketRepository.GetByIdAsync(ticket.Id);

        if (createdTicket == null)
        {
            return new ApiResponse<TicketResponseDto>(
                "Error",
                "Ticket was created but could not be retrieved",
                null
            );
        }

        return new ApiResponse<TicketResponseDto>(
            "Success",
            "Ticket created successfully",
            MapToDto(createdTicket)
        );
    }

    public async Task<ApiResponse<List<TicketResponseDto>>> GetMyTicketsAsync(
        int userId)
    {
        if (userId <= 0)
        {
            return new ApiResponse<List<TicketResponseDto>>(
                "Error",
                "Invalid user",
                null
            );
        }

        var tickets =
            await _ticketRepository.GetByCreatedByUserIdAsync(userId);

        var data = tickets.Select(MapToDto).ToList();

        return new ApiResponse<List<TicketResponseDto>>(
            "Success",
            "Your tickets retrieved successfully",
            data
        );
    }

    public async Task<ApiResponse<List<TicketResponseDto>>> GetAssignedTicketsAsync(
        int userId)
    {
        if (userId <= 0)
        {
            return new ApiResponse<List<TicketResponseDto>>(
                "Error",
                "Invalid user",
                null
            );
        }

        var tickets =
            await _ticketRepository.GetByAssignedToUserIdAsync(userId);

        var data = tickets.Select(MapToDto).ToList();

        return new ApiResponse<List<TicketResponseDto>>(
            "Success",
            "Assigned tickets retrieved successfully",
            data
        );
    }

    private static TicketResponseDto MapToDto(Ticket ticket)
    {
        return new TicketResponseDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            Priority = ticket.Priority.ToString(),
            CreatedBy = ticket.CreatedByUser.FullName,
            AssignedTo = ticket.AssignedToUser?.FullName,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ClosedAt = ticket.ClosedAt,
            ResolutionNotes = ticket.ResolutionNotes
        };
    }

    private static string GenerateTicketNumber()
    {
        return $"TKT-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}