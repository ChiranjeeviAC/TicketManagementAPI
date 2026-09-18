using TicketManagement.API.Models.Entities;

namespace TicketManagement.API.Repositories.Interfaces;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync();
}