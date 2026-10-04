using GramaMaster.Application.DTOs.Chats;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // POST: api/Chat/message
        [HttpPost("message")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendMessage(
            [FromBody] ChatRequestDto dto)
        {
            var studentId = GetStudentId();

            var result =
                await _chatService.SendMessageAsync(
                    studentId,
                    dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Chat/history
        [HttpGet("history")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHistory()
        {
            var studentId = GetStudentId();

            var result =
                await _chatService.GetHistoryAsync(studentId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Chat/session/{sessionId}
        [HttpGet("session/{sessionId:guid}")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSessionMessages(
            Guid sessionId)
        {
            var result =
                await _chatService.GetSessionMessagesAsync(sessionId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        private Guid GetStudentId()
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.Parse(studentId!);
        }
    }
}