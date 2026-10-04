using GramaMaster.API.Controllers;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Exams;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Services;
using GramaMaster.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        // POST: api/Exams/start
        [HttpPost("start")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> StartExam(
            [FromBody] StartExamDto dto)
        {
            var result =
                await _examService.StartExamAsync(
                    GetStudentId(),
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Exams/submit
        [HttpPost("submit")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SubmitExam(
            [FromBody] SubmitExamDto dto)
        {
            var result =
                await _examService.SubmitExamAsync(
                    GetStudentId(),
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Exams/attempt/{attemptId}
        [HttpGet("attempt/{attemptId:guid}")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAttempt(Guid attemptId)
        {
            var result =
                await _examService.GetAttemptAsync(
                    GetStudentId(),
                    attemptId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Exams/leaderboard/{contestId}
        [HttpGet("leaderboard/{contestId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLeaderboard(
            Guid contestId,
            [FromQuery] QueryDto query)
        {
            var result =
                await _examService.GetLeaderboardAsync(
                    contestId,
                    query);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Exams/attempt/{attemptId}/review
        [HttpGet("attempt/{attemptId:guid}/review")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ReviewAnswers(Guid attemptId)
        {
            var result =
                await _examService.ReviewAnswersAsync(attemptId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        private Guid GetStudentId()
        {
            var studentId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return Guid.Parse(studentId!);
        }
    }
}
