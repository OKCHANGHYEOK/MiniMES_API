using Microsoft.EntityFrameworkCore;
using MiniMES_API.DTOs;
using MiniMES_API.Entities;
using MiniMES_API.Infrastructure;

namespace MiniMES_API.Services.Domain;

public sealed class CompanyUserService : ICompanyUserService
{
    private readonly AppDbContext _dbContext;

    public CompanyUserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataResponse<CompanyUser_Res>> GetCompanyUsersAsync(CompanyUser_Req request, CancellationToken cancellationToken = default)
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

        return DataResponse<CompanyUser_Res>.Success(items: users.Select(user => user.ToResponse<CompanyUser_Res>()!).ToList());
    }

    public async Task<DataResponse<CompanyUser_Res>> GetByIdAsync(long companyUserId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .AsNoTracking()
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);

        return user is null
            ? DataResponse<CompanyUser_Res>.Failure(404, "회사를 찾을 수 없습니다.")
            : DataResponse<CompanyUser_Res>.Success(item: user.ToResponse<CompanyUser_Res>());
    }

    public async Task<DataResponse<CompanyUser_Res>> CreateAsync(CompanyUser_Req request, CancellationToken cancellationToken = default)
    {
        var companyExists = await _dbContext.Companies.AnyAsync(company => company.CompanyId == request.CompanyId,
                                                                  cancellationToken);
        if (!companyExists)
        {
            return DataResponse<CompanyUser_Res>.Failure(404, "소속 회사를 찾을 수 없습니다.");
        }

        var user = request.ToEntity<CompanyUser>();
        if (user is null)
        {
            return DataResponse<CompanyUser_Res>.Failure(400, "회사 사용자 정보를 생성할 수 없습니다.");
        }

        _dbContext.CompanyUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return DataResponse<CompanyUser_Res>.Success(user.ToResponse<CompanyUser_Res>(), responseCode: 201, message: "회사 사용자가 생성되었습니다.");
    }

    public async Task<DataResponse<CompanyUser_Res>> UpdateAsync(long companyUserId, CompanyUser_Req request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);
        if (user is null)
        {
            return DataResponse<CompanyUser_Res>.Failure(404, "회사 사용자를 찾을 수 없습니다.");
        }

        var companyExists = await _dbContext.Companies.AnyAsync(company => company.CompanyId == request.CompanyId,
                                                                  cancellationToken);
        if (!companyExists)
        {
            return DataResponse<CompanyUser_Res>.Failure(404, "소속 회사를 찾을 수 없습니다.");
        }

        var updatedUser = request.ToEntity<CompanyUser>();
        if (updatedUser is null)
        {
            return DataResponse<CompanyUser_Res>.Failure(400, "회사 사용자 정보를 수정할 수 없습니다.");
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
        return DataResponse<CompanyUser_Res>.Success(user.ToResponse<CompanyUser_Res>(), message: "회사 사용자가 수정되었습니다.");
    }

    public async Task<DataResponse<CompanyUser_Res>> DeleteAsync(long companyUserId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.CompanyUsers
                                   .SingleOrDefaultAsync(current => current.CompanyUserId == companyUserId,
                                                         cancellationToken);
        if (user is null)
        {
            return DataResponse<CompanyUser_Res>.Failure(404, "회사 사용자를 찾을 수 없습니다.");
        }

        _dbContext.CompanyUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return DataResponse<CompanyUser_Res>.Success(message: "회사 사용자가 삭제되었습니다.");
    }
}
