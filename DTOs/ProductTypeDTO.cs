namespace MiniMES_API.DTOs;

public class ProductTypeDTO : BaseDTO
{
    public string ProductTypeCode { get; set; } = string.Empty;
    public string ProductTypeName { get; set; } = string.Empty;
    public string ProductTypeDescription { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public sealed class ProductType_Req : ProductTypeDTO
{
}

public sealed class ProductType_Res : ProductTypeDTO
{
    public int ProductTypeId { get; set; }
}
