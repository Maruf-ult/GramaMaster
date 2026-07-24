using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Topics
{
    public class UpdateTopicDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
