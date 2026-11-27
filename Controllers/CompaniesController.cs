using Microsoft.AspNetCore.Mvc;
using MiniMES_API.DTOs;
using MiniMES_API.Services.Domain;

namespace MiniMES_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies([FromQuery] Company_Req request, CancellationToken cancellationToken)
    {
        var companies = await _companyService.GetCompaniesAsync(request, cancellationToken);
        return companies.IsSuccess ? Ok(companies) : StatusCode(companies.ResponseCode, companies);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCompany(long id, CancellationToken cancellationToken)
    {
        var company = await _companyService.GetByIdAsync(id, cancellationToken);
        return company.IsSuccess ? Ok(company) : StatusCode(company.ResponseCode, company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] Company_Req request, CancellationToken cancellationToken)
    {
        var company = await _companyService.CreateAsync(request, cancellationToken);
        return company.IsSuccess
            ? CreatedAtAction(nameof(GetCompany), new { id = company.Item!.CompanyId }, company)
            : StatusCode(company.ResponseCode, company);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCompany(long id, [FromBody] Company_Req request, CancellationToken cancellationToken)
    {
        var updated = await _companyService.UpdateAsync(id, request, cancellationToken);
        return updated.IsSuccess ? Ok(updated) : StatusCode(updated.ResponseCode, updated);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCompany(long id, CancellationToken cancellationToken)
    {
        var deleted = await _companyService.DeleteAsync(id, cancellationToken);
        return deleted.IsSuccess ? Ok(deleted) : StatusCode(deleted.ResponseCode, deleted);
    }
}
