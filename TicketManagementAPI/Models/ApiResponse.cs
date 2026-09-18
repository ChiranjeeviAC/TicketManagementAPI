namespace TicketManagement.API.Models;

public class ApiResponse<T>
{
    public string Status { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(string status, string message, T? data)
    {
        Status = status;
        Message = message;
        Data = data;
    }
}