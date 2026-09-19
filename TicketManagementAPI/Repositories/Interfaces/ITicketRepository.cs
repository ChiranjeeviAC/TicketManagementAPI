using TicketManagement.API.Models.Entities;

namespace TicketManagement.API.Repositories.Interfaces;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync();

    Task<Ticket?> GetByIdAsync(int id);

    Task<List<Ticket>> GetByCreatedByUserIdAsync(int userId);

    Task<List<Ticket>> GetByAssignedToUserIdAsync(int userId);

    Task AddAsync(Ticket ticket);

    Task SaveChangesAsync();

    Task AddStatusHistoryAsync(TicketStatusHistory history);

    Task<List<TicketStatusHistory>> GetStatusHistoryAsync(
        int ticketId);

    Task AddCommentAsync(TicketComment comment);

    Task<List<TicketComment>> GetCommentsAsync(
        int ticketId);
}