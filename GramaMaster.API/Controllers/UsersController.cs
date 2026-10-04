using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GramaMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get a user by their ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get a user by their email address.
        /// </summary>
        [HttpGet("email/{email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Check if a user exists by email.
        /// </summary>
        [HttpGet("exists/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByEmail(string email)
        {
            var result = await _userService.ExistsByEmailAsync(email);
            return Ok(result);
        }

        /// <summary>
        /// Get all users with optional search, sort, and pagination.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers([FromQuery] QueryDto query)
        {
            var result = await _userService.GetUsersAsync(query);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get users filtered by role with optional search, sort, and pagination.
        /// </summary>
        [HttpGet("role/{role}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersByRole(UserRole role, [FromQuery] QueryDto query)
        {
            var result = await _userService.GetUsersByRoleAsync(role, query);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get all teachers with optional search, sort, and pagination.
        /// </summary>
        [HttpGet("teachers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeachers([FromQuery] QueryDto query)
        {
            var result = await _userService.GetTeachersAsync(query);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get all students with optional search, sort, and pagination.
        /// </summary>
        [HttpGet("students")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudents([FromQuery] QueryDto query)
        {
            var result = await _userService.GetStudentsAsync(query);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get a teacher's full profile by user ID.
        /// </summary>
        [HttpGet("{userId:guid}/teacher-profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeacherProfile(Guid userId)
        {
            var result = await _userService.GetTeacherAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get a student's full profile by user ID.
        /// </summary>
        [HttpGet("{userId:guid}/student-profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentProfile(Guid userId)
        {
            var result = await _userService.GetStudentAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Activate or deactivate a user account.
        /// </summary>
        [HttpPatch("{userId:guid}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid userId, [FromQuery] bool isActive)
        {
            var result = await _userService.UpdateStatusAsync(userId, isActive);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Soft-delete a user account.
        /// </summary>
        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.DeleteAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
