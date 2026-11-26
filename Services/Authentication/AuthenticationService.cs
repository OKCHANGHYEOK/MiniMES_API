using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _dbContext;

    private readonly TokenService _tokenService;
    private readonly IPasswordHasher<CompanyUser> _passwordHasher;

    public AuthenticationService(AppDbContext dbContext, TokenService tokenService, IPasswordHasher<CompanyUser> passwordHasher)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<DataResponse<AuthResponse>> SetAutentication(CompanyUser_Req request)
    {
        var response = new DataResponse<AuthResponse>() { IsSuccess = false };

        var user = request.ToEntity<CompanyUser>();
        if (user is null)
        {
            response.Message = "로그인 중 오류가 발생했습니다.";
            response.ResponseCode = 500;

            return response;
        }

        var loginUser = await _dbContext.CompanyUsers.SingleOrDefaultAsync(c => c.LoginId == user.LoginId);
        if (loginUser is null)
        {
            response.Message = "해당하는 사용자를 찾을 수 없습니다.";
            response.ResponseCode = 404;

            return response;
        }

        var validatePassword = _passwordHasher.VerifyHashedPassword(user, loginUser.PasswordHash, user.PasswordOrigin);
        if (validatePassword == PasswordVerificationResult.Failed)
        {
            response.Message = "아이디 또는 비밀번호가 올바르지 않습니다.";
            response.ResponseCode = 401;

            return response;
        }

        if (!loginUser.IsActive)
        {
            response.Message = "비활성화된 사용자입니다.";
            response.ResponseCode = 403;

            return response;
        }

        if (validatePassword == PasswordVerificationResult.SuccessRehashNeeded)
        {
            loginUser.PasswordHash = _passwordHasher.HashPassword(loginUser, user.PasswordOrigin);
        }

        loginUser.LastLoginAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

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
            IsAuthenticated = true,
        };

        item.authUser.PasswordOrigin = string.Empty;
        item.authUser.PasswordHash = string.Empty;

        response.Item = item;
        response.IsSuccess = true;

        return response;
    }
}
