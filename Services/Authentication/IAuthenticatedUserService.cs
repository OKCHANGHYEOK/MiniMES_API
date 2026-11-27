using MiniMES_API.Entities;

namespace MiniMES_API.Services.Authentication;

public interface IAuthenticatedUserService
{
    public CompanyUser? GetUser();
    public void SetUser(CompanyUser user);
}
