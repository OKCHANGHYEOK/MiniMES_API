using MiniMES_API.DTOs;

namespace MiniMES_API.Services.Domain;

public interface ICompanyService
{
    Task<DataResponse<Company_Res>> GetCompaniesAsync(Company_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<Company_Res>> GetByIdAsync(long companyId, CancellationToken cancellationToken = default);

    Task<DataResponse<Company_Res>> CreateAsync(Company_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<Company_Res>> UpdateAsync(long companyId, Company_Req request, CancellationToken cancellationToken = default);

    Task<DataResponse<Company_Res>> DeleteAsync(long companyId, CancellationToken cancellationToken = default);
}
