using AutoMapper;
using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Application.DTOs.Teacher;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class TeacherProfile:Profile
    {
        public TeacherProfile()
        {
            CreateMap<RegisterTeacherDto, Teacher>();
            CreateMap<Teacher, TeacherProfileDto>();
            CreateMap<UpdateTeacherProfileDto, Teacher>();
            CreateMap<Teacher, TeacherDashboardDto>();
            CreateMap<Teacher, TeacherAnalyticsDto>();



        }
    }
}
