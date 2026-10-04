using AutoMapper;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;
using GramaMaster.Application.DTOs.Teacher;
using GramaMaster.Application.DTOs.Users;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if(user == null)
            {
                return ApiResponse<UserDto>.ErrorResponse(new List<string> { "User not found" }, "User not found");
            }
            return ApiResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(user), "User found");
        }
        public async Task<ApiResponse<UserDto>> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if(user == null)
            {
                return ApiResponse<UserDto>.ErrorResponse(new List<string> { "User not found" }, "User not found");
            }
            return ApiResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(user), "User found");
        }
        public async Task<ApiResponse<bool>> ExistsByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if(user == null)
            {
                return ApiResponse<bool>.SuccessResponse(false, "User not found");
            }
            return ApiResponse<bool>.SuccessResponse(true, "User found");
        }
        public async Task<ApiResponse<List<UserDto>>> GetUsersAsync( QueryDto query)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            if(users == null || users.Count == 0)
            {
                return ApiResponse<List<UserDto>>.ErrorResponse(new List<string> { "No users found" }, "Not Found");
            }
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return ApiResponse<List<UserDto>>.SuccessResponse(userDtos, "Users found");
        }
        public async Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(
            UserRole role,
            QueryDto query)
        {
            var users = await _unitOfWork.Users.GetUsersByRoleAsync(role, query);
            if(users == null || users.Count == 0)
            {
                return ApiResponse<List<UserDto>>.ErrorResponse(new List<string> { "No users found" }, "Not Found");
            }
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return ApiResponse<List<UserDto>>.SuccessResponse(userDtos, "Users found");
        }
        public async Task<ApiResponse<List<UserDto>>> GetTeachersAsync(
            QueryDto query)
        {
            var teachers = await _unitOfWork.Users.GetTeachersAsync(query);
            if(teachers == null || teachers.Count == 0)
            {
                return ApiResponse<List<UserDto>>.ErrorResponse(new List<string> { "No teachers found" }, "Not Found");
            }
            var teacherDtos = _mapper.Map<List<UserDto>>(teachers);
            return ApiResponse<List<UserDto>>.SuccessResponse(teacherDtos, "Teachers found");
        }

        public async Task<ApiResponse<List<UserDto>>> GetStudentsAsync(
            QueryDto query)
        {
            var students = await _unitOfWork.Users.GetStudentsAsync(query);
            if(students == null || students.Count == 0)
            {
                return ApiResponse<List<UserDto>>.ErrorResponse(new List<string> { "No students found" }, "Not Found");
            }
            var studentDtos = _mapper.Map<List<UserDto>>(students);
            return ApiResponse<List<UserDto>>.SuccessResponse(studentDtos, "Students found");
        }
        public async Task<ApiResponse<TeacherProfileDto>> GetTeacherAsync(
            Guid userId)
        {
            var teacher = await _unitOfWork.Users.GetWithTeacherAsync(userId);
            if(teacher == null) 
            {
                return ApiResponse<TeacherProfileDto>.ErrorResponse(new List<string> { "Teacher not found" }, "Not Found");
            }
            var teacherProfileDto = _mapper.Map<TeacherProfileDto>(teacher);
            return ApiResponse<TeacherProfileDto>.SuccessResponse(teacherProfileDto, "Teacher found");
            
        }

        public async Task<ApiResponse<StudentProfileDto>> GetStudentAsync( Guid userId)
        {
            var student = await _unitOfWork.Users.GetWithStudentAsync(userId);
            if(student == null) 
            {
                return ApiResponse<StudentProfileDto>.ErrorResponse(new List<string> { "Student not found" }, "Not Found");
            }
            var studentProfileDto = _mapper.Map<StudentProfileDto>(student);
            return ApiResponse<StudentProfileDto>.SuccessResponse(studentProfileDto, "Student found");
        }

        public async Task<ApiResponse<bool>> UpdateStatusAsync(
            Guid userId,
            bool isActive)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if(user == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "User not found" }, "Not Found");
            }
            user.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "User status updated");
        }
        public async Task<ApiResponse<bool>> DeleteAsync(
            Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if(user == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "User not found" }, "Not Found");
            }
            user.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "User deleted");
        }
    }
}
