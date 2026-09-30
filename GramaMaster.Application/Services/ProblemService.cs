using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Problems;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class ProblemService:IProblemService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProblemDto> _createProblemValidator;
        private readonly IValidator<UpdateProblemDto> _updateProblemValidator;

        public ProblemService(IMapper mapper, IUnitOfWork unitOfWork, IValidator<CreateProblemDto> createProblemValidator, IValidator<UpdateProblemDto> updateProblemValidator )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _createProblemValidator = createProblemValidator ?? new CreateProblemValidator();
            _updateProblemValidator = updateProblemValidator ?? new UpdateProblemValidator();
        }

        public async Task<ApiResponse<ProblemDto>> CreateProblemAsync(Guid userId, CreateProblemDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<ProblemDto>.ErrorResponse(new[] { "Payload cant be empty" }, "Invalid Request");
            }

            var validationResult = await _createProblemValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<ProblemDto>.ErrorResponse(errors, "Validation Failed");
            }
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if(user == null)
            {
                return ApiResponse<ProblemDto>.ErrorResponse(new List<string> { "User not found" }, "Request Failed");
            }

            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);
            if(topic == null)
            {
                return ApiResponse<ProblemDto>.ErrorResponse(new List<string> { "Topic not found" }, "Request Failed");
            }


            var Problem = new Problem
            {
                Id = Guid.NewGuid(),
                TopicId = dto.TopicId,
                ContestId = dto.ContestId,
                ProblemType = dto.ProblemType,
                Difficulty = dto.Difficulty,
                QuestionText = dto.QuestionText,
                CorrectAns = dto.CorrectAns,
                Explanation = dto.Explanation,
                IsAiGenerated = false
            };

            var problem = await _unitOfWork.Problems.GetProblemByProblemId(Problem.Id);

            foreach (var optionDto in dto.Options)
            {
                var option = new ProblemOptions
                {
                    Id = Guid.NewGuid(),
                    ProblemId = Problem.Id,
                    OptionText = optionDto.OptionText,
                    Problem = problem,
                    IsCorrect = optionDto.IsCorrect
                };
                Problem.ProblemOptions.Add(option);
            }
            problem.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Problems.AddAsync(problem);
            await _unitOfWork.SaveChangesAsync();

            var result = new ProblemDto
            {
                Id = problem.Id,
                Topic = topic.Name,
                ProblemType = problem.ProblemType,
                Difficulty = problem.Difficulty,
                QuestionText = problem.QuestionText,
                IsAiGenerated = problem.IsAiGenerated
            };

            return ApiResponse<ProblemDto>.SuccessResponse( result, "Problem created successfully");


        }

        public async Task<ApiResponse<bool>> UpdateProblemAsync(Guid problemId, UpdateProblemDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload cant be empty" }, "Invalid Request");
            }
            var validationResult = await _updateProblemValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<bool>.ErrorResponse(errors, "Validation Failed");
            }
            var problem = await _unitOfWork.Problems.GetProblemByProblemId(problemId);

            if (problem == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Problem not found" }, "Request Failed");
            }
            problem.TopicId = dto.TopicId;
            problem.ProblemType = dto.ProblemType;
            problem.Difficulty = dto.Difficulty;
            problem.QuestionText = dto.QuestionText;
            problem.CorrectAns = dto.CorrectAns;
            problem.Explanation = dto.Explanation;

            problem.ProblemOptions.Clear();

            foreach(var optionDto in dto.Options)
            {
                problem.ProblemOptions.Add(new ProblemOptions
                {
                    ProblemId = problem.Id,
                    OptionText = optionDto.OptionText
                });
            }

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Problem updated successfully");
            
        }

        public async Task<ApiResponse<bool>> DeleteProblemAsync(Guid problemId)
        {
            var problem = await _unitOfWork.Problems.GetProblemByProblemId(problemId);
            if(problem == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> {"Problem not found" }, "Request Failed");
            }
            var delete = await _unitOfWork.Problems.DeleteProblemAsync(problemId);
            if(delete == false)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Problem not found" }, "Request Failed");
            }
            return ApiResponse<bool>.SuccessResponse(true, "Problem deleted Successfully");
        }

        public async Task<ApiResponse<ProblemDetailsDto>> GetProblemDetailsAsync(Guid problemId)
        {
            var problem = await _unitOfWork.Problems.GetProblemByProblemId(problemId);
            if (problem == null)
            {
                return ApiResponse<ProblemDetailsDto>.ErrorResponse(new List<string> { "Problem not found" }, "Request Failed");
            }
            var problemDetails = await _unitOfWork.Problems.GetProblemWithOptionsAsync(problemId);
            ProblemDetailsDto prob = _mapper.Map<ProblemDetailsDto>(problemDetails);

            return ApiResponse<ProblemDetailsDto>.SuccessResponse(prob, "Problem Details fetched successfully");

        }

        public async Task<ApiResponse<List<ProblemDto>>> GetContestProblemsAsync(Guid contestId)
        {
            var problems = await _unitOfWork.Contests.GetContestProblemsAsync(contestId);

            if(problems == null)
            {
                return ApiResponse<List<ProblemDto>>.ErrorResponse(new List<string> {"Contest not found" }, "Request Failed");
            }
            List<ProblemDto> probs = _mapper.Map<List<ProblemDto>>(problems);
            return ApiResponse<List<ProblemDto>>.SuccessResponse(probs, "All contest problems fetched successfully");
        }

        public async Task<ApiResponse<List<ProblemDto>>> GetTeacherProblemsAsync(Guid teacherId)
        {
            var teacherProblem = await _unitOfWork.Teachers.GetProblemsCreatedAsync(teacherId);
            if(teacherProblem == null)
            {
                return ApiResponse<List<ProblemDto>>.ErrorResponse(new List<string> {"Teacher Problem not found"},"Request Failed");
            }
            List<ProblemDto> teacherProbDto = _mapper.Map<List<ProblemDto>>(teacherProblem);
            return ApiResponse<List<ProblemDto>>.SuccessResponse(teacherProbDto,"Teacher created problems fetched successfully");

        }

        public async Task<ApiResponse<List<ProblemDto>>> SearchProblemAsync(string keyword)
        {
            var problems = await _unitOfWork.Problems.SearchProblemsAsync(keyword);
            if (problems == null)
            {
                return ApiResponse<List<ProblemDto>>.ErrorResponse(new List<string> { "Problem not found" }, "Request Failed");
            }
            List<ProblemDto> probs = _mapper.Map<List<ProblemDto>>(problems);
            return ApiResponse<List<ProblemDto>>.SuccessResponse(probs, "searched completed");
        }

        public async Task<ApiResponse<List<PracticeProblemDto>>> GetPracticeProblemsAsync(Guid studentId, Guid topicId, DifficultyType difficulty, int count)
        {
            var practiceProblems = await _unitOfWork.Problems.GetPracticeProblemsAsync(studentId, topicId, difficulty, count);

            if(practiceProblems == null)
            {
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(new List<string> { "Practice Problems not found" }, "Request Failed");
            }
            List<PracticeProblemDto> pracProbs = _mapper.Map<List<PracticeProblemDto>>(practiceProblems);
            return ApiResponse<List<PracticeProblemDto>>.SuccessResponse(pracProbs, "Practice Problems fetched successfully");

        }

    }
}
