using TicketManagement.API.Models.Enums;

namespace TicketManagement.API.Models.Entities;

public class TicketStatusHistory
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public TicketStatus OldStatus { get; set; }

    public TicketStatus NewStatus { get; set; }

    public int ChangedByUserId { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Remarks { get; set; }

    public Ticket Ticket { get; set; } = null!;

    public User ChangedByUser { get; set; } = null!;
}