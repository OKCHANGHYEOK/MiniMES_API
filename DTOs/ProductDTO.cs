namespace MiniMES_API.DTOs;

public class ProductDTO : BaseDTO
{
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
    public bool IsActive { get; set; }
}

public class Product_Req : ProductDTO
{

}

public class Product_Res : ProductDTO
{
    public int ProductId { get; set; }
}
