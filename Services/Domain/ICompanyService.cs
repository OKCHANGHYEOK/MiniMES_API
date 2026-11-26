using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface ICompanyService
{
    Task<IReadOnlyList<Company_Res>> GetCompaniesAsync(Company_Req request, CancellationToken cancellationToken = default);

    Task<Company_Res?> GetByIdAsync(long companyId, CancellationToken cancellationToken = default);

    Task<Company_Res?> CreateAsync(Company_Req request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(long companyId, Company_Req request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(long companyId, CancellationToken cancellationToken = default);
}
