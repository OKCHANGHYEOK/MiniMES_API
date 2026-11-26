using Microsoft.EntityFrameworkCore;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Domain;

public sealed class CompanyService : ICompanyService
{
    private readonly AppDbContext _dbContext;

    public CompanyService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Company_Res>> GetCompaniesAsync(Company_Req request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Companies.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.CompanyCode))
        {
            query = query.Where(company => company.CompanyCode == request.CompanyCode);
        }

        if (!string.IsNullOrWhiteSpace(request.CompanyName))
        {
            query = query.Where(company => company.CompanyName.Contains(request.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(request.keyword))
        {
            query = query.Where(company => company.CompanyCode.Contains(request.keyword) ||
                                           company.CompanyName.Contains(request.keyword));
        }

        var companies = await query.OrderBy(company => company.CompanyId)
                                   .ToListAsync(cancellationToken);

        return [.. companies.Select(company => company.ToResponse<Company_Res>()!)];
    }

    public async Task<Company_Res?> GetByIdAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .AsNoTracking()
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);

        return company?.ToResponse<Company_Res>();
    }

    public async Task<Company_Res?> CreateAsync(Company_Req request, CancellationToken cancellationToken = default)
    {
        var company = request.ToEntity<Company>();
        if (company is null)
        {
            return null;
        }

        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return company.ToResponse<Company_Res>();
    }

    public async Task<bool> UpdateAsync(long companyId, Company_Req request, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);
        if (company is null)
        {
            return false;
        }

        var updatedCompany = request.ToEntity<Company>();
        if (updatedCompany is null)
        {
            return false;
        }

        company.CompanyCode = updatedCompany.CompanyCode;
        company.CompanyName = updatedCompany.CompanyName;
        company.BusinessNumber = updatedCompany.BusinessNumber;
        company.RepresentativeName = updatedCompany.RepresentativeName;
        company.BusinessType = updatedCompany.BusinessType;
        company.BusinessCategory = updatedCompany.BusinessCategory;
        company.EstablishedDate = updatedCompany.EstablishedDate;
        company.PostalCode = updatedCompany.PostalCode;
        company.Address = updatedCompany.Address;
        company.AddressDetail = updatedCompany.AddressDetail;
        company.PhoneNumber = updatedCompany.PhoneNumber;
        company.FaxNumber = updatedCompany.FaxNumber;
        company.Email = updatedCompany.Email;
        company.Website = updatedCompany.Website;
        company.IsActive = updatedCompany.IsActive;
        company.Description = updatedCompany.Description;
        company.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);
        if (company is null)
        {
            return false;
        }

        _dbContext.Companies.Remove(company);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
