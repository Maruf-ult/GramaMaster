using AutoMapper;
using GramaMaster.Application.DTOs.GrammerRules;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class GrammerRuleProfile:Profile
    {
        public GrammerRuleProfile()
        {
            CreateMap<CreateGrammarRuleDto, GrammarRule>();
            CreateMap<UpdateGrammarRuleDto, GrammarRule>();
            CreateMap<GrammarRule, GrammarRuleDto>();
        }
    }
}
