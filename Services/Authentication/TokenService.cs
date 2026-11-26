using Microsoft.IdentityModel.Tokens;
using MiniMES_API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniMES_API.Services.Authentication;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateAccessToken(CompanyUser user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt SecretKey 설정이 올바르지 않습니다.");

        var claimns = new[]
        {
            new Claim("CompanyId", user.CompanyId.ToString()),
            new Claim("CompanyUserId", user.CompanyUserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.LoginId),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Position ?? "USR"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var crendentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(jwtSection.GetValue<int>("AccessTokentMinutes"));
        var token = new JwtSecurityToken(issuer: jwtSection["Issuer"], audience: jwtSection["Audience"], claims: claimns, expires: expires, signingCredentials: crendentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
