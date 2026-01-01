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


        //Register
        //POST /api/auth/register
        //Creates a new account and returns JWT token
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var register = await _authService.RegisterAsync(dto.ToEntity());

            if (register == null)
            {
                return Conflict(new { message = "Email already registered" }); // 409 conflict
            }

            return Created("", register.ToDto()); // 201 created

        }


        //Login
        //POST /api/auth/login
        //Authenticates user and return JWT token
        [HttpPost("login")]
         public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _authService.LoginAsync(dto.ToEntity());

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" }); // 401 Unauthorized
            }

            return Ok(result?.ToDto()); // 200 OK status
        }


        //Refresh Token ~ will exchange a valid refresh token for a new access token ~ refresh token validation
        //POST /api/auth/refresh
        [HttpPost("refresh")]
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
                return Unauthorized(new { message = "Invalid or expired refresh token" }); // 401 Unauthorized ~ not 400 validation problem
            }

            return Ok(result);
        }

        //Logout ~ single device & invalidate the current refresh token
        //POST /api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
        {
            //stateless logout for refresh tokens
            await _authService.LogoutAsync(dto.RefreshToken);

            return NoContent(); // 204 no content success & no body
        }


        //Logout ~ all devices
        //POST /api/auth/logout-everywhere
        [HttpPost("logout-everywhere")]
        [Authorize]
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
