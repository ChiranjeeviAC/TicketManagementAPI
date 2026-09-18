using TicketManagement.API.DTOs.Tickets;

namespace TicketManagement.API.Services.Interfaces;

public interface ITicketService
{
    Task<List<TicketResponseDto>> GetAllAsync();
}