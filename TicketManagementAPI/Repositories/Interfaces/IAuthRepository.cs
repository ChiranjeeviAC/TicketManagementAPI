using TicketManagement.API.Models.Entities;

namespace TicketManagement.API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddUserAsync(User user);
}