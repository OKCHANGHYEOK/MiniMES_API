using Microsoft.AspNetCore.Mvc;
using MiniMES_API.DTOs;
using MiniMES_API.Services.Authentication;

namespace MiniMES_API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private IAuthenticationService _authenticationService;
        
    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CompanyUser_Req user)
    {
        var authentication = await _authenticationService.SetAutentication(user);           
        if (authentication.Item is null || !authentication.IsSuccess)
        {
            return authentication.ResponseCode switch
            {
                404 => NotFound(authentication.Message),
                500 => StatusCode(StatusCodes.Status500InternalServerError, new { authentication.Message }),
                _ => StatusCode(StatusCodes.Status400BadRequest)
            };
        }

        return Ok(authentication.Item);
    }
}
