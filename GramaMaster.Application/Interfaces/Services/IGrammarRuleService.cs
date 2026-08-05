using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.GrammerRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IGrammarRuleService
    {
        Task<ApiResponse<GrammarRuleDto>> CreateAsync(
            CreateGrammarRuleDto dto);

        Task<ApiResponse<bool>> UpdateAsync(
            Guid id,
            UpdateGrammarRuleDto dto);

        Task<ApiResponse<bool>> DeleteAsync(Guid id);

        Task<List<GrammarRuleDto>> GetTopicRulesAsync(Guid topicId);

        Task<GrammarRuleDto> GetByIdAsync(Guid id);
    }
}
