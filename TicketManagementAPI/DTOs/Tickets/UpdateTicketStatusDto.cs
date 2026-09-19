namespace TicketManagement.API.DTOs.Tickets;

public class UpdateTicketStatusDto
{
    public string Status { get; set; } = string.Empty;

    public string? ResolutionNotes { get; set; }
}