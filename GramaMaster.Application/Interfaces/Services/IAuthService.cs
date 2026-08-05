using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> RegisterStudentAsync(RegisterStudentDto dto);

        Task<ApiResponse<LoginResponseDto>> RegisterTeacherAsync(RegisterTeacherDto dto);

        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);

        Task<ApiResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);

        Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto);

        Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    }
}
