using TicketManagement.API.DTOs.Tickets;
using TicketManagement.API.Models;

namespace TicketManagement.API.Services.Interfaces;

public interface ITicketService
{
    Task<ApiResponse<List<TicketResponseDto>>> GetAllAsync();

    Task<ApiResponse<TicketResponseDto>> GetByIdAsync(int id);

    Task<ApiResponse<TicketResponseDto>> CreateAsync(
        CreateTicketDto dto,
        int userId);

    Task<ApiResponse<List<TicketResponseDto>>> GetMyTicketsAsync(
        int userId);

    Task<ApiResponse<List<TicketResponseDto>>> GetAssignedTicketsAsync(
        int userId);

    Task<ApiResponse<TicketResponseDto>> UpdateAsync(
       int id,
       UpdateTicketDto dto,
       int userId,
       string role);

    Task<ApiResponse<TicketResponseDto>> AssignSolverAsync(
    int ticketId,
    int solverId);

    Task<ApiResponse<TicketResponseDto>> UpdateStatusAsync(
    int ticketId,
    UpdateTicketStatusDto dto,
    int userId,
    string role);

    Task<ApiResponse<object>> AddCommentAsync(
    int ticketId,
    AddCommentDto dto,
    int userId);
}