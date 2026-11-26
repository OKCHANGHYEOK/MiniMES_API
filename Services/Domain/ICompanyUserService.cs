using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface ICompanyUserService
{
    Task<DataResponse<CompanyUser_Res>> GetCompanyUsersAsync(CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<CompanyUser_Res>> GetByIdAsync(long companyUserId, CancellationToken cancellationToken = default);

    Task<DataResponse<CompanyUser_Res>> CreateAsync(CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<CompanyUser_Res>> UpdateAsync(long companyUserId, CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<CompanyUser_Res>> DeleteAsync(long companyUserId, CancellationToken cancellationToken = default);
}
