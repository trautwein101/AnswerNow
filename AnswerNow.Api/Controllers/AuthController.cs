using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnswerNow.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Registers a new user account and returns an authentication response as JWT token.
        /// </summary>
        /// <param name="dto">Registration payload.</param>
        /// <response code="201">Returns the authentication response for the newly created user.</response>
        /// <response code="400">If the payload is invalid.</response>
        /// <response code="409">If the email is already registered.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var register = await _authService.RegisterAsync(dto.ToEntity());

            if (register == null)
            {
                return Conflict(new { message = "Email already registered" });
            }

            return Created("", register.ToDto());

        }


        /// <summary>
        /// Authenticates a user and returns an authentication response.
        /// </summary>
        /// <param name="dto">Login payload.</param>
        /// <response code="200">Returns the authentication response.</response>
        /// <response code="400">If the payload is invalid.</response>
        /// <response code="401">If credentials are invalid.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _authService.LoginAsync(dto.ToEntity());

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result?.ToDto());
        }


        /// <summary>
        /// Exchanges a valid refresh token for a new access token.
        /// </summary>
        /// <param name="dto">Refresh token request payload.</param>
        /// <response code="200">Returns the new authentication response.</response>
        /// <response code="400">If the payload is invalid.</response>
        /// <response code="401">If the refresh token is invalid or expired.</response>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);

            //token invalid, expired or already in use
            if(result == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Logs out single device of the current session by revoking the provided refresh token.
        /// </summary>
        /// <param name="dto">Refresh token request payload.</param>
        /// <response code="204">Logout succeeded.</response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
        {
            //stateless logout for refresh tokens
            await _authService.LogoutAsync(dto.RefreshToken);

            return NoContent();
        }


        /// <summary>
        /// Logs out the current user from all devices by revoking all refresh tokens.
        /// </summary>
        /// <response code="204">Logout everywhere succeeded.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("logout-everywhere")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogoutEverywhere()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);
            await _authService.LogoutEverywhereAsync(userId);

            return NoContent();
        }

    }
}
