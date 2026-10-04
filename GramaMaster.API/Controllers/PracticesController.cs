using GramaMaster.Application.DTOs.Practice;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class PracticesController : ControllerBase
    {
        private readonly IPracticeService _practiceService;

        public PracticesController(IPracticeService practiceService)
        {
            _practiceService = practiceService;
        }

        // POST: api/Practices/start
        [HttpPost("start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> StartPractice(
            [FromBody] StartPracticeDto dto)
        {
            var result =
                await _practiceService.StartPracticeAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/Practices/submit
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitPractice(
            [FromBody] PracticeSubmissionDto dto)
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _practiceService.SubmitPracticeAsync(
                    studentId.Value,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Practices/history
        [HttpGet("history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHistory()
        {
            var studentId = GetUserId();

            if (studentId == null)
                return Unauthorized();

            var result =
                await _practiceService.GetHistoryAsync(
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