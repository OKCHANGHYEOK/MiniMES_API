using Microsoft.AspNetCore.Mvc;
using MiniMES_API.DTOs;
using MiniMES_API.Services;

namespace MiniMES_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] Product_Req request, CancellationToken cancellationToken)
    {
        var ret = await _productService.GetProductsAsync(request, cancellationToken);
        if (ret.Count == 0)
        {
            return NoContent();
        }

        return Ok(ret);
    }

    // 단건 조회
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    // 등록
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product_Req request, CancellationToken cancellationToken)
    {
        var ret = await _productService.CreateAsync(request, cancellationToken);
        if (ret is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(CreateProduct),
                               new { id = ret.ProductId },
                               ret);
    }

    // 수정
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product_Req request, int id, CancellationToken cancellationToken)
    {
        var ret = await _productService.UpdateAsync(id, request, cancellationToken);

        return ret ? NoContent() : NotFound();
    }

    // 삭제
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
