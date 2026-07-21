using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Student
{
    public class UpdateStudentProfileDto
    {
        public string? ProfileImageUrl { get; set; }

        public string InstitutionName { get; set; } = string.Empty;

        public StudyGroup Group { get; set; }

        public EducationBoard Board { get; set; }
    }
}
