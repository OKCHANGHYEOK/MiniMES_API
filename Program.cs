using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniMES_API.Infrastructure;
using MiniMES_API.Services.Authentication;
using MiniMES_API.Services.Domain;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

// JWT 인증
SetJwtAutentication(builder);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // 모든 도메인 허용 (개발용)
              .AllowAnyMethod()   // GET, POST 등 모든 메서드 허용
              .AllowAnyHeader();  // 모든 헤더 허용
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseNpgsql(connectionString)
                                                   .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SetJwtAutentication(WebApplicationBuilder builder)
{
    if (builder is null)
        return;

    var jwtSection = builder.Configuration.GetSection("Jwt");
    var secretKey = builder.Configuration["Jwt:SecretKey"];

    if (string.IsNullOrWhiteSpace(secretKey))
    {
        throw new InvalidOperationException("등록된 Jwt 비밀키가 존재하지 않거나 올바르지 않습니다.");
    }

    // 기본 인증 방식으로 JWT Bearer 인증을 사용하도록 설정
    // 클라이언트는 API 요청 시 Authorization 헤더에 Bearer 토큰을 전달해야 함
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => 
                    {
                        options.TokenValidationParameters = new TokenValidationParameters {
                        
                            ValidateIssuerSigningKey = true, // 토큰이 정상적인 발급자에 의해 생성되었는지 서명을 검증
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // 서명 검증에 사용할 비밀키
                            ValidateIssuer = true, // 토큰을 발급한 주체가 신뢰할 수 있는 발급자인지 확인
                            ValidIssuer = jwtSection["Issuer"],  // JWT에 포함된 iss(issuer) 값과 비교

                            ValidateAudience = true, // 해당 토큰이 현재 API를 대상으로 발급된 것인지 확인
                            ValidAudience = jwtSection["Audience"], // 검증 시 허용할 대상자, JWT에 포함된 aud(audience) 값과 비교

                            ValidateLifetime = true, // 토큰의 만료 시간(exp)이 지났는지 확인, 아직 유효 기간이 시작되지 않은 경우(nbf)도 확인
                            ClockSkew = TimeSpan.Zero // 만료 시간 검증 시 허용할 시간 오차, 0으로 설정하면 별도의 시간 오차를 허용하지 않음
                        };
                    });

    builder.Services.AddAuthorization();
}