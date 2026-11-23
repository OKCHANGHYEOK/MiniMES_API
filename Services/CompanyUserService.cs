using Microsoft.EntityFrameworkCore;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services;

public sealed class CompanyUserService : ICompanyUserService
{
    private readonly AppDbContext _dbContext;

    public CompanyUserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CompanyUser_Res>> GetCompanyUsersAsync(CompanyUser_Req request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.CompanyUsers.AsNoTracking().AsQueryable();

        if (request.CompanyId > 0)
        {
            query = query.Where(user => user.CompanyId == request.CompanyId);
        }

        if (!string.IsNullOrWhiteSpace(request.LoginId))
        {
            query = query.Where(user => user.LoginId == request.LoginId);
        }

        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            query = query.Where(user => user.UserName.Contains(request.UserName));
        }

        if (!string.IsNullOrWhiteSpace(request.keyword))
        {
            query = query.Where(user => user.LoginId.Contains(request.keyword) ||
                                        user.UserName.Contains(request.keyword));
        }

        var users = await query.OrderBy(user => user.CompanyUserId)
                               .ToListAsync(cancellationToken);

        return [.. users.Select(user => user.ToResponse<CompanyUser_Res>()!)];
    }

    public async Task<CompanyUser_Res?> GetByIdAsync(long companyUserId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .AsNoTracking()
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);

        return user?.ToResponse<CompanyUser_Res>();
    }

    public async Task<CompanyUser_Res?> CreateAsync(CompanyUser_Req request, CancellationToken cancellationToken = default)
    {
        var companyExists = await _dbContext.Companies.AnyAsync(company => company.CompanyId == request.CompanyId,
                                                                  cancellationToken);
        if (!companyExists)
        {
            return null;
        }

        var user = request.ToEntity<CompanyUser>();
        if (user is null)
        {
            return null;
        }

        _dbContext.CompanyUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.ToResponse<CompanyUser_Res>();
    }

    public async Task<bool> UpdateAsync(long companyUserId, CompanyUser_Req request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);
        if (user is null)
        {
            return false;
        }

        var companyExists = await _dbContext.Companies.AnyAsync(company => company.CompanyId == request.CompanyId,
                                                                  cancellationToken);
        if (!companyExists)
        {
            return false;
        }

        var updatedUser = request.ToEntity<CompanyUser>();
        if (updatedUser is null)
        {
            return false;
        }

        user.CompanyId = updatedUser.CompanyId;
        user.LoginId = updatedUser.LoginId;
        if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
        {
            user.PasswordHash = updatedUser.PasswordHash;
        }
        user.UserName = updatedUser.UserName;
        user.EmployeeNumber = updatedUser.EmployeeNumber;
        user.Department = updatedUser.Department;
        user.Position = updatedUser.Position;
        user.JobTitle = updatedUser.JobTitle;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Email = updatedUser.Email;
        user.HireDate = updatedUser.HireDate;
        user.ResignationDate = updatedUser.ResignationDate;
        user.IsActive = updatedUser.IsActive;
        user.Description = updatedUser.Description;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(long companyUserId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);
        if (user is null)
        {
            return false;
        }

        _dbContext.CompanyUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
