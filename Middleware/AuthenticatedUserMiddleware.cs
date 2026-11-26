using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MiniMES_API.Infrastructure;
using MiniMES_API.Services.Authentication;

namespace MiniMES_API.Middleware;

public sealed class AuthenticatedUserMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticatedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthenticatedUserService authenticatedUserService, AppDbContext dbContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var authenticatedUser = authenticatedUserService.GetUser();
            if (authenticatedUser is null)
            {
                var companyIdClaim = context.User.FindFirstValue("CompanyId");
                var companyUserIdClaim = context.User.FindFirstValue("CompanyUserId");

                if (!long.TryParse(companyIdClaim, out var companyId) ||
                    !long.TryParse(companyUserIdClaim, out var companyUserId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                authenticatedUser = await dbContext.CompanyUsers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(
                        user => user.CompanyId == companyId &&
                                user.CompanyUserId == companyUserId &&
                                user.IsActive,
                        context.RequestAborted);

                if (authenticatedUser is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                authenticatedUserService.SetUser(authenticatedUser);
            }
        }

        await _next(context);
    }
}
