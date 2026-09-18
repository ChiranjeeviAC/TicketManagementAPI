namespace TicketManagement.API.DTOs.Tickets;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;
}