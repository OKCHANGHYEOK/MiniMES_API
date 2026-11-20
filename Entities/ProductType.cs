namespace MiniMES_API.Entities;

public sealed class ProductType : BaseEntity
{
    public int ProductTypeId { get; set; }
    public string ProductTypeCode { get; set; } = string.Empty;
    public string ProductTypeName { get; set; } = string.Empty;
    public string ProductTypeDescription { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
