using AutoMapper;
using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Application.DTOs.Student;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class StudentProfile:Profile
    {
        public StudentProfile()
        {
            CreateMap<RegisterStudentDto, Student>();
            CreateMap<Student, StudentProfileDto>();
            CreateMap<UpdateStudentProfileDto, Student>();
            CreateMap<Student, StudentDashboardDto>();
            CreateMap<Student, StudentAnalyticsDto>();
            CreateMap<Student, StudentProgressDto>();

        }
    }
}
