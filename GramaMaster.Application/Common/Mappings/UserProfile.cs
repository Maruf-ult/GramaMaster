using AutoMapper;
using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterStudentDto,User>();
            CreateMap<RegisterTeacherDto, User>();
            CreateMap<User, LoginResponseDto>();
        }
    }
}
