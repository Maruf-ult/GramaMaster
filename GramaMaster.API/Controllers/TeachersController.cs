using GramaMaster.Application.DTOs.Teacher;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // ============================================================
        // Get Teacher Profile
        // GET: api/Teachers/profile
        // ============================================================

        /// <summary>
        /// Gets the profile of the currently logged-in teacher.
        /// </summary>
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfile()
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teacherService.GetProfileAsync(teacherId.Value);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Update Teacher Profile
        // PUT: api/Teachers/profile
        // ============================================================

        /// <summary>
        /// Updates the profile of the currently logged-in teacher.
        /// </summary>
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateTeacherProfileDto dto)
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teacherService.UpdateProfileAsync(
                    teacherId.Value,
                    dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Teacher Dashboard
        // GET: api/Teachers/dashboard
        // ============================================================

        /// <summary>
        /// Gets dashboard information for the currently logged-in teacher.
        /// </summary>
        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboard()
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teacherService.GetDashboardAsync(
                    teacherId.Value);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Teacher Analytics
        // GET: api/Teachers/analytics
        // ============================================================

        /// <summary>
        /// Gets analytics for the currently logged-in teacher.
        /// </summary>
        [HttpGet("analytics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAnalytics()
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teacherService.GetAnalyticsAsync(
                    teacherId.Value);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Get Logged-in User ID
        // ============================================================

        private Guid? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }

            return null;
        }
    }
}