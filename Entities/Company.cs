namespace MiniMES_API.Entities;


public class Company : BaseEntity
{
    public long CompanyId { get; set; }

    // 기업 기본 정보
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? BusinessNumber { get; set; }
    public string? RepresentativeName { get; set; }

    // 사업 정보
    public string? BusinessType { get; set; }
    public string? BusinessCategory { get; set; }
    public DateOnly? EstablishedDate { get; set; }

    // 주소 정보
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? AddressDetail { get; set; }

    // 연락처 정보
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    // 관리 정보
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
