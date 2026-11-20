using Microsoft.EntityFrameworkCore;
using MiniMES_API.Common;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services;

public sealed class ProductTypeService : IProductTypeService
{
    private readonly AppDbContext _dbContext;

    public ProductTypeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProductType_Res>> GetProductTypesAsync(ProductType_Req request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ProductTypes.AsNoTracking()
                                           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.ProductTypeCode))
        {
            query = query.Where(x => x.ProductTypeCode == request.ProductTypeCode);
        }

        if (!string.IsNullOrWhiteSpace(request.ProductTypeName))
        {
            query = query.Where(x => x.ProductTypeName == request.ProductTypeName);
        }

        if (!string.IsNullOrWhiteSpace(request.keyword))
        {
            query = query.Where(x => x.ProductTypeName.Contains(request.keyword) ||
                                     x.ProductTypeCode.Contains(request.keyword));
        }

        return [.. query.Select(x => x.ToResponse<ProductType_Res>()!)];
    }

    public async Task<ProductType_Res?> GetByIdAsync(int productTypeId, CancellationToken cancellationToken = default)
    {
        var query = await _dbContext.ProductTypes.AsNoTracking().SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);

        return query?.ToResponse<ProductType_Res>();
    }

    public async Task<ProductType_Res?> CreateAsync(ProductType_Req request, CancellationToken cancellationToken = default)
    {
        var item = request.ToEntity<ProductType>();        
        if (item is null)
        {
            return null;
        }

        var query = await _dbContext.ProductTypes.AddAsync(item, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return item.ToResponse<ProductType_Res>();
    }

    public async Task<bool> UpdateAsync(int productTypeId, ProductType_Req request, CancellationToken cancellationToken = default)
    {
        var oldItem = await _dbContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);
        if (oldItem is null)
            return false;

        var newItem = request.ToEntity<ProductType>();
        if (newItem is null)
            return false;

        EntityMapper.Map(oldItem, newItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int productTypeId, CancellationToken cancellationToken = default)
    {
        var delItem = await _dbContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);
        if (delItem is null)
            return false;

        _dbContext.ProductTypes.Remove(delItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
