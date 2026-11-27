using Microsoft.AspNetCore.Mvc;
using MiniMES_API.DTOs;
using MiniMES_API.Services.Domain;

namespace MiniMES_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductTypesController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypesController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    // 전체 조회
    [HttpGet]
    public async Task<IActionResult> GetProductTypes([FromQuery] ProductType_Req request, CancellationToken cancellationToken)
    {
        var ret = await _productTypeService.GetProductTypesAsync(request, cancellationToken);
        return ret.IsSuccess ? Ok(ret) : StatusCode(ret.ResponseCode, ret);
    }

    // 단건 조회
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductType(int id, CancellationToken cancellationToken)
    {
        var ret = await _productTypeService.GetByIdAsync(id, cancellationToken);
        return ret.IsSuccess ? Ok(ret) : StatusCode(ret.ResponseCode, ret);
    }

    // 등록
    [HttpPost]
    public async Task<IActionResult> CreateProductType([FromBody] ProductType_Req request, CancellationToken cancellationToken)
    {
        var ret = await _productTypeService.CreateAsync(request, cancellationToken);
        return ret.IsSuccess
            ? CreatedAtAction(nameof(GetProductType), new { id = ret.Item!.ProductTypeId }, ret)
            : StatusCode(ret.ResponseCode, ret);
    }

    // 수정
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductType([FromBody] ProductType_Req request, int id, CancellationToken cancellationToken)
    {
        var ret = await _productTypeService.UpdateAsync(id, request, cancellationToken);

        return ret.IsSuccess ? Ok(ret) : StatusCode(ret.ResponseCode, ret);
    }

    // 삭제
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductType(int id, CancellationToken cancellationToken)
    {
        var ret = await _productTypeService.DeleteAsync(id, cancellationToken);

        return ret.IsSuccess ? Ok(ret) : StatusCode(ret.ResponseCode, ret);
    }
}
