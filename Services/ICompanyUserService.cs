using MiniMES_API.DTOs;

namespace MiniMES_API.Services;

public interface ICompanyUserService
{
    Task<IReadOnlyList<CompanyUser_Res>> GetCompanyUsersAsync(CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<CompanyUser_Res?> GetByIdAsync(long companyUserId, CancellationToken cancellationToken = default);

    Task<CompanyUser_Res?> CreateAsync(CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(long companyUserId, CompanyUser_Req request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(long companyUserId, CancellationToken cancellationToken = default);
}
