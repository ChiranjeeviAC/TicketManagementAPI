using Microsoft.EntityFrameworkCore;
using TicketManagement.API.Data;
using TicketManagement.API.Models.Entities;
using TicketManagement.API.Repositories.Interfaces;

namespace TicketManagement.API.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .ToListAsync();
    }
}