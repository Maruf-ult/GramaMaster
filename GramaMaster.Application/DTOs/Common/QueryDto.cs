using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Common
{
    public class QueryDto
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
