using MiniMES_API.Entities;

namespace MiniMES_API.DTOs;

public class AuthResponse
{
    public CompanyUser? authUser { get; set; }
    public string? TokenValue { get; set; }
    public bool IsAuthenticated { get; set; }
}
