namespace MiniMES_API.DTOs;

public sealed class CompanyUser_Req : BaseDTO
{
    public int CompanyId { get; set; }
    public string LoginId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? EmployeeNumber { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? ResignationDate { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}

public sealed class CompanyUser_Res : BaseDTO
{
    public int CompanyUserId { get; set; }
    public int CompanyId { get; set; }
    public string LoginId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? EmployeeNumber { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? ResignationDate { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}