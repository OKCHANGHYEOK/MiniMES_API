namespace MiniMES_API.Entities;

public class CompanyUser : BaseEntity
{
    public long CompanyUserId { get; set; }
    public long CompanyId { get; set; }

    // 계정 정보
    public string LoginId { get; set; } = string.Empty;
    public string PasswordOrigin { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // 기본 정보
    public string UserName { get; set; } = string.Empty;
    public string? EmployeeNumber { get; set; }

    // 소속 및 직무 정보
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? JobTitle { get; set; }

    // 연락처 정보
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    // 재직 정보
    public DateOnly? HireDate { get; set; }
    public DateOnly? ResignationDate { get; set; }

    // 계정 관리 정보
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Property
    public Company Company { get; set; } = null!;
}
