using AutoMapper;
using GramaMaster.Application.DTOs.Contests;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class ContestProfile:Profile
    {
        public ContestProfile()
        {
            CreateMap<CreateContestDto, Contest>();
            CreateMap<UpdateContestDto, Contest>();
            CreateMap<Contest, ContestDto>();
            CreateMap<Contest, ContestAnalyticsDto>();
            CreateMap<Contest, ContestCardDto>();
            CreateMap<Contest, ContestListDto>();
            CreateMap<Contest, ContestDetailsDto>();

        }
    }
}
