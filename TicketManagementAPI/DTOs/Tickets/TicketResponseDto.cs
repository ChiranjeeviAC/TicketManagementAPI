namespace TicketManagement.API.DTOs.Tickets;

public class TicketResponseDto
{
    public int Id { get; set; }

    public string TicketNumber { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public string? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public string? ResolutionNotes { get; set; }
}