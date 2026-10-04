using GramaMaster.Application.DTOs.GrammerRules;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GrammarsController : ControllerBase
    {
        private readonly IGrammarRuleService _grammarRuleService;

        public GrammarsController(IGrammarRuleService grammarRuleService)
        {
            _grammarRuleService = grammarRuleService;
        }

        // POST: api/Grammars
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(
            [FromBody] CreateGrammarRuleDto dto)
        {
            var result =
                await _grammarRuleService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // PUT: api/Grammars/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateGrammarRuleDto dto)
        {
            var result =
                await _grammarRuleService.UpdateAsync(id, dto);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // DELETE: api/Grammars/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result =
                await _grammarRuleService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Grammars/topic/{topicId}
        [HttpGet("topic/{topicId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTopicRules(Guid topicId)
        {
            var result =
                await _grammarRuleService.GetTopicRulesAsync(topicId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // GET: api/Grammars/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result =
                await _grammarRuleService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}