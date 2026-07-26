using AutoMapper;
using GramaMaster.Application.DTOs.Team;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class TeamProfile:Profile
    {
        public TeamProfile()
        {
            CreateMap<CreateTeamDto, Team>();
            CreateMap<UpdateTeamDto, Team>();
            CreateMap<Team, TeamDto>();
            CreateMap<TeamMember, TeamMemberDto>();
        }
    }
}
