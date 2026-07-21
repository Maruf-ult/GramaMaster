using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class TeacherProfileDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public double SscResult { get; set; }

        public StudyGroup SscGroup { get; set; }

        public EducationBoard SscBoard { get; set; }

        public double HscResult { get; set; }

        public StudyGroup HscGroup { get; set; }

        public EducationBoard HscBoard { get; set; }

        public string Department { get; set; } = string.Empty;

        public string University { get; set; } = string.Empty;
    }
}
