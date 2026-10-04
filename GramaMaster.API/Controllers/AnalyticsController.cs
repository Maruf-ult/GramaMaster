using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/Analytics/admin-dashboard
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var result =
                await _analyticsService.GetAdminDashboardAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Analytics/topics
        [HttpGet("topics")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTopicStatistics()
        {
            var result =
                await _analyticsService.GetTopicStatisticsAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Analytics/performance-trend
        [HttpGet("performance-trend")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPerformanceTrend()
        {
            var studentId = GetUserId();

            var result =
                await _analyticsService.GetPerformanceTrendAsync(
                    studentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Analytics/monthly-progress
        [HttpGet("monthly-progress")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMonthlyProgress()
        {
            var studentId = GetUserId();

            var result =
                await _analyticsService.GetMonthlyProgressAsync(
                    studentId);

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