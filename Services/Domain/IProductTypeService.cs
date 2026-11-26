using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface IProductTypeService
{
    Task<IReadOnlyList<ProductType_Res>> GetProductTypesAsync(ProductType_Req request, CancellationToken cancellationToken = default);

    Task<ProductType_Res?> GetByIdAsync(int productTypeId, CancellationToken cancellationToken = default);

    Task<ProductType_Res?> CreateAsync(ProductType_Req productType, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int productTypeId, ProductType_Req productType, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int productTypeId, CancellationToken cancellationToken = default);
}
