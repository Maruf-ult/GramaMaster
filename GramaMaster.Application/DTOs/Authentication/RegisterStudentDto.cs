using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Authentication
{
    public class RegisterStudentDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string InstitutionName { get; set; } = string.Empty;
        public CurriculumType CurriculumType { get; set; }
        public StudyGroup Group { get; set; }
        public int ExamBatchYear { get; set; }
        public EducationBoard Board { get; set; }


    }
}
