using Microsoft.EntityFrameworkCore;
using MiniMES_API.Common;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services;

public sealed class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;

    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product_Res>> GetProductsAsync(Product_Req request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Products
                              .AsNoTracking()
                              .AsQueryable();

        if (request.ProductTypeId > 0)
        {
            query = query.Where(product => product.ProductTypeId == request.ProductTypeId);
        }

        if (!string.IsNullOrWhiteSpace(request.ProductCode))
        {
            query = query.Where(product => product.ProductCode == request.ProductCode);
        }

        if (!string.IsNullOrWhiteSpace(request.ProductName))
        {
            query = query.Where(product => product.ProductName.Contains(request.ProductName));
        }

        var products = await query.OrderBy(product => product.ProductId)
                                  .ToListAsync(cancellationToken);

        return [.. products.Select(product => product.ToResponse<Product_Res>()!)];
    }

    public async Task<Product_Res?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(current => current.ProductId == productId,
                                                            cancellationToken);

        return product?.ToResponse<Product_Res>();
    }

    public async Task<Product_Res?> CreateAsync(Product_Req request, CancellationToken cancellationToken = default)
    {
        var product = request.ToEntity<Product>();
        if (product is null)
            return null;

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.ToResponse<Product_Res>()!;
    }

    public async Task<bool> UpdateAsync(int productId, Product_Req request, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _dbContext.Products
                                              .SingleOrDefaultAsync(current => current.ProductId == productId, cancellationToken);

        if (existingProduct is null)
        {
            return false;
        }

        var product = request.ToEntity<Product>();
        if (product is null)
            return false;
        
        EntityMapper.Map(existingProduct, product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(current => current.ProductId == productId, cancellationToken);

        if (product is null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
