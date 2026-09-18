using TicketManagement.API.DTOs.Tickets;
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

    public async Task<List<TicketResponseDto>> GetAllAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();

        return tickets.Select(t => new TicketResponseDto
        {
            Id = t.Id,
            TicketNumber = t.TicketNumber,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedBy = t.CreatedByUser.FullName,
            AssignedTo = t.AssignedToUser?.FullName,
            CreatedAt = t.CreatedAt
        }).ToList();
    }
}