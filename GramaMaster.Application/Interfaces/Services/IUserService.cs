using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Users; 
using GramaMaster.Domain.Enums;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IUserService
    {
        // Admin: Get all users with search, filter by role, pagination
        Task<ApiResponse<PagedResultDto<UserDto>>> GetUsersAsync(QueryDto query, UserRole? role = null);

        // Admin / User: Get user profile by ID
        Task<ApiResponse<UserDto>> GetByIdAsync(Guid userId);

        // Admin: Activate / Deactivate user account (e.g. ban spammer or teacher approval)
        Task<ApiResponse<bool>> ToggleUserStatusAsync(Guid userId, bool isActive);

        // Admin: Delete user
        Task<ApiResponse<bool>> DeleteUserAsync(Guid userId);
    }
}
