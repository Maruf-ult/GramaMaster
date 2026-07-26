using AutoMapper;
using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class ProblemProfile:Profile
    {
        public ProblemProfile()
        {
            CreateMap<CreateProblemDto,Problem>();
            CreateMap<UpdateProblemDto, Problem>();
            CreateMap<Problem, ProblemDto>();
            CreateMap<Problem, ProblemDetailsDto>();
            CreateMap<PracticeProblemDto, Problem>();
            CreateMap<ProblemOptions, ProblemOptionDto>();


        }
    }
}
