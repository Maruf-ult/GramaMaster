using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Contests;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContestsController : ControllerBase
    {
        private readonly IContestService _contestService;

        public ContestsController(IContestService contestService)
        {
            _contestService = contestService;
        }

        // POST: api/Contests
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(
            [FromBody] CreateContestDto dto)
        {
            var userId = GetUserId();

            var result =
                await _contestService.CreateContestAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // PUT: api/Contests/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateContestDto dto)
        {
            var result =
                await _contestService.UpdateContestAsync(id, dto);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // DELETE: api/Contests/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result =
                await _contestService.DeleteContestAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Contests/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result =
                await _contestService.GetContestDetailsAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Contests/global
        [HttpGet("global")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGlobalContests(
            [FromQuery] QueryDto query)
        {
            var result =
                await _contestService.GetGlobalContestsAsync(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Contests/teacher
        [HttpGet("teacher")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTeacherContests(
            [FromQuery] QueryDto query)
        {
            var teacherId = GetUserId();

            var result =
                await _contestService.GetTeacherContestsAsync(
                    teacherId,
                    query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Contests/student/available
        [HttpGet("student/available")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudentAvailableContests(
            [FromQuery] QueryDto query)
        {
            var studentId = GetUserId();

            var result =
                await _contestService.GetStudentAvailableContestsAsync(
                    studentId,
                    query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Contests/{id}/leaderboard
        [HttpGet("{id:guid}/leaderboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLeaderboard(Guid id)
        {
            var result =
                await _contestService.GetLeaderboardAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Contests/{id}/analytics
        [HttpGet("{id:guid}/analytics")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAnalytics(Guid id)
        {
            var result =
                await _contestService.GetAnalyticsAsync(id);

            if (!result.Success)
                return NotFound(result);

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