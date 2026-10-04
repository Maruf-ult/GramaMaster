using GramaMaster.Application.DTOs.AI;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        // POST: api/AI/generate-questions
        [HttpPost("generate-questions")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateQuestions(
            [FromBody] GenerateQuestionsDto dto)
        {
            var result =
                await _aiService.GenerateQuestionsAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/AI/explain-answer/{answerSubmissionId}
        [HttpGet("explain-answer/{answerSubmissionId:guid}")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExplainAnswer(
            Guid answerSubmissionId)
        {
            var result =
                await _aiService.ExplainAnswerAsync(
                    answerSubmissionId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/AI/weak-topics
        [HttpGet("weak-topics")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AnalyzeWeakTopics()
        {
            var studentId = GetUserId();

            var result =
                await _aiService.AnalyzeWeakTopicsAsync(
                    studentId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/AI/recommendations
        [HttpGet("recommendations")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RecommendTopics()
        {
            var studentId = GetUserId();

            var result =
                await _aiService.RecommendTopicsAsync(
                    studentId);

            if (!result.Success)
                return BadRequest(result);

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