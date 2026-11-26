using Microsoft.EntityFrameworkCore;
using MiniMES_API.Common;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Domain;

public sealed class ProductTypeService : IProductTypeService
{
    private readonly AppDbContext _dbContext;

    public ProductTypeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataResponse<ProductType_Res>> GetProductTypesAsync(ProductType_Req request, CancellationToken cancellationToken = default)
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

        var productTypes = await query.ToListAsync(cancellationToken);
        return DataResponse<ProductType_Res>.Success(items: productTypes.Select(x => x.ToResponse<ProductType_Res>()!).ToList());
    }

    public async Task<DataResponse<ProductType_Res>> GetByIdAsync(int productTypeId, CancellationToken cancellationToken = default)
    {
        var query = await _dbContext.ProductTypes.AsNoTracking().SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);

        return query is null
            ? DataResponse<ProductType_Res>.Failure(404, "상품 유형을 찾을 수 없습니다.")
            : DataResponse<ProductType_Res>.Success(item: query.ToResponse<ProductType_Res>());
    }

    public async Task<DataResponse<ProductType_Res>> CreateAsync(ProductType_Req request, CancellationToken cancellationToken = default)
    {
        var item = request.ToEntity<ProductType>();        
        if (item is null)
        {
            return DataResponse<ProductType_Res>.Failure(400, "상품 유형 정보를 생성할 수 없습니다.");
        }

        var query = await _dbContext.ProductTypes.AddAsync(item, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<ProductType_Res>.Success(item.ToResponse<ProductType_Res>(), responseCode: 201, message: "상품 유형이 생성되었습니다.");
    }

    public async Task<DataResponse<ProductType_Res>> UpdateAsync(int productTypeId, ProductType_Req request, CancellationToken cancellationToken = default)
    {
        var oldItem = await _dbContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);
        if (oldItem is null)
            return DataResponse<ProductType_Res>.Failure(404, "상품 유형을 찾을 수 없습니다.");

        var newItem = request.ToEntity<ProductType>();
        if (newItem is null)
            return DataResponse<ProductType_Res>.Failure(400, "상품 유형 정보를 수정할 수 없습니다.");

        EntityMapper.Map(oldItem, newItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<ProductType_Res>.Success(oldItem.ToResponse<ProductType_Res>(), message: "상품 유형이 수정되었습니다.");
    }

    public async Task<DataResponse<ProductType_Res>> DeleteAsync(int productTypeId, CancellationToken cancellationToken = default)
    {
        var delItem = await _dbContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == productTypeId, cancellationToken);
        if (delItem is null)
            return DataResponse<ProductType_Res>.Failure(404, "상품 유형을 찾을 수 없습니다.");

        _dbContext.ProductTypes.Remove(delItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<ProductType_Res>.Success(message: "상품 유형이 삭제되었습니다.");
    }
}
