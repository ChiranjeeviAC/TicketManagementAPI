namespace TicketManagement.API.DTOs.Tickets;

public class TicketActivityDto
{
    public List<CommentResponseDto> Comments { get; set; } = new();
    public List<StatusHistoryResponseDto> StatusHistory { get; set; } = new();
}

public class CommentResponseDto
{
    public string User { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class StatusHistoryResponseDto
{
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public string? Remarks { get; set; }
}