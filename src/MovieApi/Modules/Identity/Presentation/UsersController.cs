using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Modules.Identity.Application;
using MovieApi.Modules.Identity.Contracts.Users;
using MovieApi.Modules.Identity.Mapping;
using MovieApi.SharedKernel.Authorization;

namespace MovieApi.Modules.Identity.Presentation;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UsersController(IUserRepository users) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserResponse> GetMe()
    {
        var subject = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(subject, out var userId))
        {
            return Unauthorized();
        }

        var user = users.GetById(userId);
        return user is null ? Unauthorized() : Ok(user.ToResponse());
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType<IReadOnlyCollection<UserResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<IReadOnlyCollection<UserResponse>> GetAll()
    {
        var response = users.GetAll().Select(user => user.ToResponse()).ToArray();
        return Ok(response);
    }
}
