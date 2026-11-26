using Microsoft.AspNetCore.Mvc;
using MiniMES_API.DTOs;
using MiniMES_API.Services.Domain;

namespace MiniMES_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CompanyUsersController : ControllerBase
{
    private readonly ICompanyUserService _companyUserService;

    public CompanyUsersController(ICompanyUserService companyUserService)
    {
        _companyUserService = companyUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanyUsers([FromQuery] CompanyUser_Req request, CancellationToken cancellationToken)
    {
        var users = await _companyUserService.GetCompanyUsersAsync(request, cancellationToken);
        return users.Count == 0 ? NoContent() : Ok(users);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCompanyUser(long id, CancellationToken cancellationToken)
    {
        var user = await _companyUserService.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyUser([FromBody] CompanyUser_Req request, CancellationToken cancellationToken)
    {
        var user = await _companyUserService.CreateAsync(request, cancellationToken);
        return user is null ? BadRequest() : CreatedAtAction(nameof(GetCompanyUser), new { id = user.CompanyUserId }, user);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCompanyUser(long id, [FromBody] CompanyUser_Req request, CancellationToken cancellationToken)
    {
        var updated = await _companyUserService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCompanyUser(long id, CancellationToken cancellationToken)
    {
        var deleted = await _companyUserService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
