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

    public async Task<DataResponse<Company_Res>> GetCompaniesAsync(Company_Req request, CancellationToken cancellationToken = default)
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

        return DataResponse<Company_Res>.Success(items: companies.Select(company => company.ToResponse<Company_Res>()!).ToList());
    }

    public async Task<DataResponse<Company_Res>> GetByIdAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .AsNoTracking()
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);

        return company is null
            ? DataResponse<Company_Res>.Failure(404, "회사를 찾을 수 없습니다.")
            : DataResponse<Company_Res>.Success(item: company.ToResponse<Company_Res>());
    }

    public async Task<DataResponse<Company_Res>> CreateAsync(Company_Req request, CancellationToken cancellationToken = default)
    {
        var company = request.ToEntity<Company>();
        if (company is null)
        {
            return DataResponse<Company_Res>.Failure(400, "회사 정보를 생성할 수 없습니다.");
        }

        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<Company_Res>.Success(company.ToResponse<Company_Res>(), responseCode: 201, message: "회사가 생성되었습니다.");
    }

    public async Task<DataResponse<Company_Res>> UpdateAsync(long companyId, Company_Req request, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);
        if (company is null)
        {
            return DataResponse<Company_Res>.Failure(404, "회사를 찾을 수 없습니다.");
        }

        var updatedCompany = request.ToEntity<Company>();
        if (updatedCompany is null)
        {
            return DataResponse<Company_Res>.Failure(400, "회사 정보를 수정할 수 없습니다.");
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
        return DataResponse<Company_Res>.Success(company.ToResponse<Company_Res>(), message: "회사가 수정되었습니다.");
    }

    public async Task<DataResponse<Company_Res>> DeleteAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                                       .SingleOrDefaultAsync(current => current.CompanyId == companyId,
                                                             cancellationToken);
        if (company is null)
        {
            return DataResponse<Company_Res>.Failure(404, "회사를 찾을 수 없습니다.");
        }

        _dbContext.Companies.Remove(company);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return DataResponse<Company_Res>.Success(message: "회사가 삭제되었습니다.");
    }
}
