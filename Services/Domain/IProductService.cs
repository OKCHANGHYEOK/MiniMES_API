using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface IProductService
{
    Task<DataResponse<Product_Res>> GetProductsAsync(Product_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<Product_Res>> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    Task<DataResponse<Product_Res>> CreateAsync(Product_Req product, CancellationToken cancellationToken = default);

    Task<DataResponse<Product_Res>> UpdateAsync(int productId, Product_Req product, CancellationToken cancellationToken = default);

    Task<DataResponse<Product_Res>> DeleteAsync(int productId, CancellationToken cancellationToken = default);
}
