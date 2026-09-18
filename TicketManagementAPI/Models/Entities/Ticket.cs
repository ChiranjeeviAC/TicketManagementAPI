using TicketManagement.API.Models.Enums;

namespace TicketManagement.API.Models.Entities;

public class Ticket
{
    public int Id { get; set; }

    public string TicketNumber { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }

    public TicketPriority Priority { get; set; }

    public int CreatedByUserId { get; set; }

    public int? AssignedToUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public string? ResolutionNotes { get; set; }

    public User CreatedByUser { get; set; } = null!;

    public User? AssignedToUser { get; set; }
}