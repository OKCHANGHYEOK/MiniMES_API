using MiniMES_API.DTOs;

namespace MiniMES_API.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product_Res>> GetProductsAsync(Product_Req request, CancellationToken cancellationToken = default);

    Task<Product_Res?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    Task<Product_Res?> CreateAsync(Product_Req product, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int productId, Product_Req product, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int productId, CancellationToken cancellationToken = default);
}
