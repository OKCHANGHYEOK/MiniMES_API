using MiniMES_API.DTOs;
using MiniMES_API.Entities;

namespace MiniMES_API.Services.Authentication;

public interface IAuthenticationService
{
    Task<DataResponse<AuthResponse>> SetAutentication(CompanyUser_Req user);
}
