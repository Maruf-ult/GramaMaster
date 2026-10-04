using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register/student
        [HttpPost("register/student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterStudent(
            [FromBody] RegisterStudentDto dto)
        {
            var result =
                await _authService.RegisterStudentAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Auth/register/teacher
        [HttpPost("register/teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterTeacher(
            [FromBody] RegisterTeacherDto dto)
        {
            var result =
                await _authService.RegisterTeacherAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            var result =
                await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        // POST: api/Auth/change-password
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto dto)
        {
            var userId = GetUserId();

            var result =
                await _authService.ChangePasswordAsync(
                    userId,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Auth/forgot-password
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            var result =
                await _authService.ForgotPasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Auth/reset-password
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            var result =
                await _authService.ResetPasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Auth/refresh-token
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenDto dto)
        {
            var result =
                await _authService.RefreshTokenAsync(dto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.Parse(userId!);
        }
    }
}