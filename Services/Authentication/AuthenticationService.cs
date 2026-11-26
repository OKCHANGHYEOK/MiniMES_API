using Microsoft.EntityFrameworkCore;
using MiniMES_API.DTOs;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _dbContext;

    private readonly TokenService _tokenService;

    public AuthenticationService(AppDbContext dbContext, TokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<DataResponse<AuthResponse>> SetAutentication(CompanyUser_Req user)
    {
        var response = new DataResponse<AuthResponse>() { IsSuccess = false };

        var loginUser = await _dbContext.CompanyUsers.AsNoTracking()
                                                     .SingleOrDefaultAsync(c => c.CompanyUserId == user.CompanyId && c.PasswordHash == user.PasswordHash);

        if (loginUser is null)
        {
            response.Message = "해당하는 사용자를 찾을 수 없습니다.\n아이디 또는 비밀번호를 확인해주세요.";
            response.ResponseCode = 404;

            return response;
        }

        var token = _tokenService.CreateAccessToken(loginUser);
        if (string.IsNullOrWhiteSpace(token))
        {
            response.Message = "토큰 생성중 오류가 발생했습니다.";
            response.ResponseCode = 500;
            
            return response;
        }

        var item = new AuthResponse
        {
            authUser = loginUser,
            TokenValue = token,
        };

        response.Item = item;
        response.IsSuccess = true;

        return response;
    }
}
