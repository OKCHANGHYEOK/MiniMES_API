using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface IProductTypeService
{
    Task<DataResponse<ProductType_Res>> GetProductTypesAsync(ProductType_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<ProductType_Res>> GetByIdAsync(int productTypeId, CancellationToken cancellationToken = default);

    Task<DataResponse<ProductType_Res>> CreateAsync(ProductType_Req productType, CancellationToken cancellationToken = default);

    Task<DataResponse<ProductType_Res>> UpdateAsync(int productTypeId, ProductType_Req productType, CancellationToken cancellationToken = default);

    Task<DataResponse<ProductType_Res>> DeleteAsync(int productTypeId, CancellationToken cancellationToken = default);
}
