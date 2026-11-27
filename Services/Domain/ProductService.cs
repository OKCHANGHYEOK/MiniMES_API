using Microsoft.EntityFrameworkCore;
using MiniMES_API.Common;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Domain;

public sealed class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;

    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataResponse<Product_Res>> GetProductsAsync(Product_Req request, CancellationToken cancellationToken = default)
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

        return DataResponse<Product_Res>.Success(items: products.Select(product => product.ToResponse<Product_Res>()!).ToList());
    }

    public async Task<DataResponse<Product_Res>> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(current => current.ProductId == productId,
                                                            cancellationToken);

        return product is null
            ? DataResponse<Product_Res>.Failure(404, "상품을 찾을 수 없습니다.")
            : DataResponse<Product_Res>.Success(item: product.ToResponse<Product_Res>());
    }

    public async Task<DataResponse<Product_Res>> CreateAsync(Product_Req request, CancellationToken cancellationToken = default)
    {
        var product = request.ToEntity<Product>();
        if (product is null)
            return DataResponse<Product_Res>.Failure(400, "상품 정보를 생성할 수 없습니다.");

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<Product_Res>.Success(product.ToResponse<Product_Res>(), responseCode: 201, message: "상품이 생성되었습니다.");
    }

    public async Task<DataResponse<Product_Res>> UpdateAsync(int productId, Product_Req request, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _dbContext.Products
                                              .SingleOrDefaultAsync(current => current.ProductId == productId, cancellationToken);

        if (existingProduct is null)
        {
            return DataResponse<Product_Res>.Failure(404, "상품을 찾을 수 없습니다.");
        }

        var product = request.ToEntity<Product>();
        if (product is null)
            return DataResponse<Product_Res>.Failure(400, "상품 정보를 수정할 수 없습니다.");
        
        EntityMapper.Map(existingProduct, product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<Product_Res>.Success(existingProduct.ToResponse<Product_Res>(), message: "상품이 수정되었습니다.");
    }

    public async Task<DataResponse<Product_Res>> DeleteAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(current => current.ProductId == productId, cancellationToken);

        if (product is null)
        {
            return DataResponse<Product_Res>.Failure(404, "상품을 찾을 수 없습니다.");
        }

        _dbContext.Products.Remove(product);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<Product_Res>.Success(message: "상품이 삭제되었습니다.");
    }
}
