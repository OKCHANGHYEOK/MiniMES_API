namespace MiniMES_API.DTOs;

public class CompanyDTO : BaseDTO
{
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? BusinessNumber { get; set; }
    public string? RepresentativeName { get; set; }
    public string? BusinessType { get; set; }
    public string? BusinessCategory { get; set; }
    public DateOnly? EstablishedDate { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? AddressDetail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}

public sealed class Company_Req : CompanyDTO
{
}

public sealed class Company_Res : CompanyDTO
{
    public long CompanyId { get; set; }
}