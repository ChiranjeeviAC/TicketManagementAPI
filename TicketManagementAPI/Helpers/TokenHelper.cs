using Microsoft.AspNetCore.Http;

namespace TicketManagement.API.Helpers;

public static class TokenHelper
{
    public static bool HasToken(HttpRequest request)
    {
        if (!request.Headers.TryGetValue("Authorization", out var authorization))
        {
            return false;
        }

        var value = authorization.ToString();

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (!value.StartsWith("Bearer ",
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var token = value["Bearer ".Length..].Trim();

        return !string.IsNullOrWhiteSpace(token);
    }
}