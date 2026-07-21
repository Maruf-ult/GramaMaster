using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class UpdateTeacherProfileDto
    {
        public string? ProfileImageUrl { get; set; }

        public string Department { get; set; } = string.Empty;

        public string University { get; set; } = string.Empty;
    }
}
