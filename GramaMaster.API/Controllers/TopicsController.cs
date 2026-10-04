using GramaMaster.Application.DTOs.Topics;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        ///<summary>
        ///Create a new topic
        ///</summary>
        [HttpPost]
        [Authorize(Roles ="Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicDto dto)
        {
            var result = await _topicService.CreateTopicAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update an existing topic.
        /// </summary>

        [HttpPut("{topicId:guid}")]
        [Authorize(Roles ="Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTopic(Guid topicId, [FromBody] UpdateTopicDto dto)
        {
            var result = await _topicService.UpdateTopicAsync(topicId, dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        ///<summary>
        ///Delete a topic
        /// </summary>
        [HttpDelete("{topicId:guid}")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>DeleteTopic(Guid topicId)
        {
            var result = await _topicService.DeleteTopicAsync(topicId);

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        ///<summary>
        ///Get all topics.
        /// </summary>

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTopics()
        {
            var result = await _topicService.GetAllTopicAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a topic by Id
        /// </summary>

        [HttpGet("{topicId:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopicById(Guid topicId)
        {
            var result = await _topicService.GetTopicByIdAsync(topicId);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);

        }

        /// <summary>
        /// Get all topics for a specific curriculum.
        /// </summary>
        [HttpGet("curriculum/{curriculumId:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopicsByCurriculum(
            Guid curriculumId)
        {
            var result = await _topicService
                .GetTopicByCurriculumAsync(curriculumId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }


    }
}
