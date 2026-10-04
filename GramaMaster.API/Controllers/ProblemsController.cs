using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProblemsController : ControllerBase
    {
        private readonly IProblemService _problemService;

        public ProblemsController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        // POST: api/Problems
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProblem(
            [FromBody] CreateProblemDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var result =
                await _problemService.CreateProblemAsync(
                    userId.Value,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // PUT: api/Problems/{problemId}
        [HttpPut("{problemId:guid}")]
        [Authorize(Roles = "Teacher,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProblem(
            Guid problemId,
            [FromBody] UpdateProblemDto dto)
        {
            var result =
                await _problemService.UpdateProblemAsync(
                    problemId,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // DELETE: api/Problems/{problemId}
        [HttpDelete("{problemId:guid}")]
        [Authorize(Roles = "Teacher,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProblem(
            Guid problemId)
        {
            var result =
                await _problemService.DeleteProblemAsync(
                    problemId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Problems/{problemId}
        [HttpGet("{problemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProblemDetails(
            Guid problemId)
        {
            var result =
                await _problemService.GetProblemDetailsAsync(
                    problemId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Problems/contest/{contestId}
        [HttpGet("contest/{contestId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetContestProblems(
            Guid contestId)
        {
            var result =
                await _problemService.GetContestProblemsAsync(
                    contestId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Problems/teacher
        [HttpGet("teacher")]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTeacherProblems()
        {
            var teacherId = GetUserId();

            if (teacherId == null)
                return Unauthorized();

            var result =
                await _problemService.GetTeacherProblemsAsync(
                    teacherId.Value);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Problems/search?keyword=preposition
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SearchProblems(
            [FromQuery] string keyword)
        {
            var result =
                await _problemService.SearchProblemAsync(
                    keyword);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Problems/practice?topicId=...&difficulty=...&count=10
        [HttpGet("practice")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPracticeProblems(
            [FromQuery] Guid topicId,
            [FromQuery] DifficultyType difficulty,
            [FromQuery] int count)
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _problemService.GetPracticeProblemsAsync(
                    studentId.Value,
                    topicId,
                    difficulty,
                    count);

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