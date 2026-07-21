using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

            public static ApiResponse<T> SuccessResponse(T data, string message = "")
            {
                return new ApiResponse<T>
                {
                    Success = true,
                    Message = message,
                    Data = data
                };
            }

           public static ApiResponse<T> ErrorResponse(IEnumerable<string> errors, string message = "Request Failed")
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = message,
                    Errors = errors.ToList()
                };
            }
        

    }
}
