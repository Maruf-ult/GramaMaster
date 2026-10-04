using GramaMaster.Application.DTOs.Student;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students/profile
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfile()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result = await _studentService.GetProfileAsync(studentId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // PUT: api/Students/profile
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateStudentProfileDto dto)
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _studentService.UpdateProfileAsync(
                    studentId.Value,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Students/dashboard
        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboard()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _studentService.GetDashboardAsync(
                    studentId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Students/analytics
        [HttpGet("analytics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAnalytics()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _studentService.GetAnalyticsAsync(
                    studentId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Students/progress
        [HttpGet("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProgress()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _studentService.GetProgressAsync(
                    studentId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Students/topic-progress
        [HttpGet("topic-progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTopicProgress()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _studentService.GetTopicProgressAsync(
                    studentId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        private Guid? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
                return userId;

            return null;
        }
    }
}