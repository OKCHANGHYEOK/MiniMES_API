using MiniMES_API.Entities;

namespace MiniMES_API.Services.Authentication;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    private CompanyUser? _loginUser { get; set; }

    public CompanyUser? GetUser()
    {
        return _loginUser;
    }

    public void SetUser(CompanyUser user)
    {
        _loginUser = user;
    }
}
