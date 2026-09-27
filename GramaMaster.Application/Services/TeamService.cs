using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Team;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Team;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class TeamService:ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTeamDto> _createTeamValidator;
        private readonly IValidator<UpdateTeamDto> _updateTeamValidator;
        private readonly IValidator<JoinTeamDto> _joinTeamValidator;


        public TeamService(IMapper mapper,IUnitOfWork unitOfWork,IValidator<CreateTeamDto>?createTeamValidator,IValidator<UpdateTeamDto>?updateTeamValidator,IValidator<JoinTeamDto>? joinTeamValidator  )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _createTeamValidator = createTeamValidator ?? new CreateTeamValidator();
            _updateTeamValidator = updateTeamValidator ?? new UpdateTeamValidator();
            _joinTeamValidator = joinTeamValidator ?? new JoinTeamValidator();
        }

        private static class JoinCodeGenerator
        {
            private static readonly char[] chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

            public static string GenerateCode(int length = 7)
            {
                byte[] data = new byte[length];
                using (var crypto = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    crypto.GetBytes(data);
                }
                var result = new System.Text.StringBuilder(length);
                foreach(byte b in data)
                {
                    result.Append(chars[b % chars.Length]);
                }
                return result.ToString();
            }

        }

        public async Task<ApiResponse<TeamDto>> CreateTeamAsync( Guid teacherId,CreateTeamDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<TeamDto>.ErrorResponse(new[] { "Payload cant be empty" }, "Invalid Request");
            }
            
            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);

            if(teacher == null)
            {
                return ApiResponse<TeamDto>.ErrorResponse(new List<string> { "Teacher not found" }, "Request Failed");
            }

            var validationResult = await _createTeamValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<TeamDto>.ErrorResponse(errors, "Validation Failed");
            }

            var teamName = dto.Name;
            var teamDescription = dto?.Description;
            string safeJoinCode = JoinCodeGenerator.GenerateCode(7);

            var newTeam = new TeamDto
            {
                Id = Guid.NewGuid(),
                TeacherId = teacher.Id,
                TeacherName = teacher.User.FullName,
                Name = teamName,
                Description = teamDescription,
                JoinCode = safeJoinCode,
                MemberCount = 0,
                Members = new(),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Teams.AddAsync(newTeam);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<TeamDto>.SuccessResponse(newTeam,"Team created Successfully");


        }

        public async Task<ApiResponse<bool>> UpdateTeamAsync(Guid teamId,UpdateTeamDto dto)
        {
            
            if(dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload cant be null" }, "Invalid Request");
            }
            
            var team = await _unitOfWork.Teams.GetByIdAsync(teamId);

            if(team == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Team not found" }, "Request Failed");
            }

            var validationResult = await _updateTeamValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<bool>.ErrorResponse(errors,"Validation Failed");
            }

            team.Name = dto.Name;
            team.Description = dto?.Description;

            await _unitOfWork.Teams.AddAsync(team);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Team successfully updated");

        }

        public async Task<ApiResponse<bool>> DeleteTeamAsync(Guid teamId)
        {
            var team = await _unitOfWork.Teams.GetByIdAsync(teamId);

            if (team == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Team not found" }, "Request Failed");
            }

            _unitOfWork.Teams.Delete(team);

            return ApiResponse<bool>.SuccessResponse(true, "Team deleted successfully");

        }

        public async Task<ApiResponse<bool>> JoinTeamAsync(Guid studentId,JoinTeamDto dto)
        {
            
            if(dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload cant be null" }, "Invalid Request");
            }
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
            if(student == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Student not found" }, "Request Failed");
            }
            var validationResult = await _joinTeamValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<bool>.ErrorResponse(errors, "Validation Failed");
            }
            var team = await _unitOfWork.Students.GetByJoinCodeAsync(dto.JoinCode);

            if(team == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Invalid join code. team not found" }, "Request Failed");
            }

            var alreadyMember = team.TeamMembers.Any(x => x.StudentId == student.Id && !x.IsDeleted);

            if (alreadyMember)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "you are already part of this team", "Request Failed" });
            }
            var teamMember = new TeamMember
            {
                TeamId = team.Id,
                StudentId = student.Id,
                JoinedAt = DateTime.UtcNow
            };

            await _unitOfWork.TeamMembers.AddAsync(teamMember);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true,"successfully joined the team");

        }

        public async Task<ApiResponse<TeamDto>> GetTeamAsync(Guid teamId)
        {
            var team = await _unitOfWork.Teams.GetByIdAsync(teamId);

            if (team == null)
            {
                return ApiResponse<TeamDto>.ErrorResponse(new List<string> { "Team not found" }, "Request Failed");
            }
            TeamDto teamDto = _mapper.Map<TeamDto>(team);

            return ApiResponse<TeamDto>.SuccessResponse(teamDto, "Teams fetched successfully");

        }

        public async Task<ApiResponse<List<TeamDto>>> GetTeacherTeamsAsync(Guid teacherId)
        {
            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);
            if(teacher == null)
            {
                return ApiResponse<List<TeamDto>>.ErrorResponse(new List<string> { "Teacher not found" }, "Request Failed");
            }

            var teams = await _unitOfWork.Teachers.GetTeamsByTeacherId(teacherId);

            

            if(teams == null)
            {
                return ApiResponse<List<TeamDto>>.ErrorResponse(new List<string> { "No avaiable teams" }, "Not Found");
            }
            List<TeamDto> teamDtos = _mapper.Map<List<TeamDto>>(teams);

            return ApiResponse<List<TeamDto>>.SuccessResponse(teamDtos, "Teachers teams fetched successfully");

        }

        public async Task<ApiResponse<List<TeamDto>>> GetStudentTeamsAsync(Guid studentId)
        {
            var student = await _unitOfWork.Users.GetWithStudentAsync(studentId);
            if (student == null)
            {
                return ApiResponse<List<TeamDto>>.ErrorResponse(new List<string> { "Student not found" }, "Request Failed");
            }
            var teams = await _unitOfWork.Students.GetTeamsByStudentIdAsync(student.Id);
            List<TeamDto> teamDtos = _mapper.Map<List<TeamDto>>(teams);

            return ApiResponse<List<TeamDto>>.SuccessResponse(teamDtos, "Student Teams fetched successfully");
        }

        public async Task<ApiResponse<List<TeamMemberDto>>> GetMembersAsync(Guid teamId)
        {

            var teamMembers = await _unitOfWork.TeamMembers.GetByIdAsync(teamId);

            if(teamMembers == null)
            {
                return ApiResponse<List<TeamMemberDto>>.ErrorResponse(new List<String> { "No teamembers joined yet" }, "Request Failed");
            }

            List<TeamMemberDto> membersDto = _mapper.Map<List<TeamMemberDto>>(teamMembers);

            return ApiResponse<List<TeamMemberDto>>.SuccessResponse(membersDto, "Teammembers fetched successfully");

        }

        public async Task<ApiResponse<bool>> RemoveStudentAsync(Guid teamId,Guid studentId)
        {
            var team = await _unitOfWork.Teams.GetByIdAsync(teamId);
            
            if(team == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<String> { "Team not found" }, "Request Failed");
            }

            var delete = await _unitOfWork.Teachers.RemoveStudentByIdAsync(studentId);

            if(delete == false)
            {
                return ApiResponse<bool>.ErrorResponse(new List<String> { "Student is not in this team" }, "Request Failed");
            }
            
            return ApiResponse<bool>.SuccessResponse(true, "Student deleted successfully");

        }
    }
}
