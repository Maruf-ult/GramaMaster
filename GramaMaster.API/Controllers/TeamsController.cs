using GramaMaster.Application.DTOs.Team;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // ============================================================
        // Create Team
        // POST: api/Teams
        // ============================================================

        /// <summary>
        /// Creates a new team for the logged-in teacher.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTeam(
            [FromBody] CreateTeamDto dto)
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result = await _teamService.CreateTeamAsync(
                teacherId.Value,
                dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Update Team
        // PUT: api/Teams/{teamId}
        // ============================================================

        /// <summary>
        /// Updates an existing team.
        /// </summary>
        [HttpPut("{teamId:guid}")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateTeam(
            Guid teamId,
            [FromBody] UpdateTeamDto dto)
        {
            var result = await _teamService.UpdateTeamAsync(
                teamId,
                dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Delete Team
        // DELETE: api/Teams/{teamId}
        // ============================================================

        /// <summary>
        /// Deletes a team.
        /// </summary>
        [HttpDelete("{teamId:guid}")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTeam(
            Guid teamId)
        {
            var result = await _teamService.DeleteTeamAsync(teamId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Get Team
        // GET: api/Teams/{teamId}
        // ============================================================

        /// <summary>
        /// Gets a team by its ID.
        /// </summary>
        [HttpGet("{teamId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTeam(
            Guid teamId)
        {
            var result = await _teamService.GetTeamAsync(teamId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Get Teacher Teams
        // GET: api/Teams/teacher
        // ============================================================

        /// <summary>
        /// Gets all teams created by the logged-in teacher.
        /// </summary>
        [HttpGet("teacher")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTeacherTeams()
        {
            var teacherId = GetUserId();

            if (teacherId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teamService.GetTeacherTeamsAsync(teacherId.Value);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Get Student Teams
        // GET: api/Teams/student
        // ============================================================

        /// <summary>
        /// Gets all teams joined by the logged-in student.
        /// </summary>
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudentTeams()
        {
            var studentId = GetUserId();

            if (studentId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teamService.GetStudentTeamsAsync(studentId.Value);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Join Team
        // POST: api/Teams/join
        // ============================================================

        /// <summary>
        /// Allows the logged-in student to join a team using a join code.
        /// </summary>
        [HttpPost("join")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> JoinTeam(
            [FromBody] JoinTeamDto dto)
        {
            var studentId = GetUserId();

            if (studentId == null)
            {
                return Unauthorized();
            }

            var result =
                await _teamService.JoinTeamAsync(
                    studentId.Value,
                    dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Get Team Members
        // GET: api/Teams/{teamId}/members
        // ============================================================

        /// <summary>
        /// Gets all members of a team.
        /// </summary>
        [HttpGet("{teamId:guid}/members")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMembers(
            Guid teamId)
        {
            var result =
                await _teamService.GetMembersAsync(teamId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // ============================================================
        // Remove Student From Team
        // DELETE: api/Teams/{teamId}/students/{studentId}
        // ============================================================

        /// <summary>
        /// Removes a student from a team.
        /// </summary>
        [HttpDelete("{teamId:guid}/students/{studentId:guid}")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveStudent(
            Guid teamId,
            Guid studentId)
        {
            var result =
                await _teamService.RemoveStudentAsync(
                    teamId,
                    studentId);

            if (!result.Success)
            {
                return BadRequest(result);
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