namespace MiniMES_API.Entities;

public sealed class Product : BaseEntity
{
    public int ProductId { get; set; }
    public int ProductTypeId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? ProductMaterial { get; set; }
    public decimal? ProductWidth { get; set; }
    public decimal? ProductHeight { get; set; }
    public decimal? ProductWeight { get; set; }
    public decimal? ProductThickness { get; set; }
    public decimal? ProductUnitCost { get; set; }
    public byte[]? ProductImage { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsActive { get; set; }
}
