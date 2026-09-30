using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Exams;
using GramaMaster.Application.DTOs.Practice;
using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Practice;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class PracticeService:IPracticeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<StartPracticeDto> _startPracticeValidator;
        private readonly IValidator<PracticeSubmissionDto> _practiceSubmissionValidator;

        public PracticeService(IMapper mapper,IUnitOfWork unitOfWork,IValidator<StartPracticeDto> startPracticeValidator,IValidator<PracticeSubmissionDto> practiceSubmissionValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _startPracticeValidator = startPracticeValidator ;
            _practiceSubmissionValidator = practiceSubmissionValidator ?? new PracticeSubmissionValidator();
        }

        public async Task<ApiResponse<List<PracticeProblemDto>>> StartPracticeAsync(StartPracticeDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(new[] { "Payload cant be empty", "Invalid Request" });
            }
            var validationResult = await _startPracticeValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(errors,"Validation Failed");
            }
            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);

            if (topic == null || topic.IsDeleted)
            {
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(new[] { "Topic not found" }, "Request Failed");
            }

            if(topic.CurriculumId != dto.CurriculumId)
            {
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(
            new[] { "Topic does not belong to the selected curriculum" },
            "Invalid Request");
            }

            var problems = await _unitOfWork.Problems.GetPracticeProblemsAsync(dto.TopicId, dto.CurriculumId, dto.Difficulty, dto.QuestionCount);

            if (problems == null || !problems.Any())
            {
                return ApiResponse<List<PracticeProblemDto>>.ErrorResponse(
                    new[] { "No practice problems found for the selected criteria" },
                    "No Problems Found");
            }

            var practiceProblems = problems.Select(problem => new PracticeProblemDto
            {
                Id = problem.Id,
                TopicId = problem.TopicId,
                TopicName = topic.Name,
                ProblemType = problem.ProblemType,
                Difficulty = problem.Difficulty,
                QuestionText = problem.QuestionText,
                Options = problem.ProblemOptions
                .Where(option => !option.IsDeleted)
                .Select(option => new ProblemOptionDto
                {
                    Id = option.Id,
                    OptionText = option.OptionText
                })
                .ToList()

            }).ToList();
       
            return ApiResponse<List<PracticeProblemDto>>.SuccessResponse(practiceProblems,"Practice started successfully");
        }
        public async Task<ApiResponse<PracticeResultDto>> SubmitPracticeAsync(Guid studentId, PracticeSubmissionDto dto)
        {

            if(dto == null)
            {
                return ApiResponse<PracticeResultDto>.ErrorResponse(new[] {"Payload cant be null"},"Invalid Request");
            }

            var validationResult = await _practiceSubmissionValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<PracticeResultDto>.ErrorResponse(errors, "Validation Failed");
            }

            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
            if(student == null)
            {
                return ApiResponse<PracticeResultDto>.ErrorResponse(new List<string> {"Student not found" }, "Request Failed");
            }

            if (!dto.Answers.Any())
            {
                return ApiResponse<PracticeResultDto>.ErrorResponse(
                    new[] { "No answers submitted" },
                    "Invalid Request");
            }

            var problemIds = dto.Answers
                .Select(x => x.ProblemId)
                .Distinct()
                .ToList();

            var problems = await _unitOfWork.Problems
                .GetProblemsForPracticeSubmissionAsync(problemIds);

            if (problems.Count != problemIds.Count)
            {
                return ApiResponse<PracticeResultDto>.ErrorResponse(
                    new[] { "One or more problems could not be found" },
                    "Invalid Request");
            }
            var reviews = new List<AnswerReviewDto>();
            var submissions = new List<AnswerSubmission>();

            int correctAnswers = 0;

            foreach (var answer in dto.Answers)
            {
                var problem = problems.FirstOrDefault(
                    x => x.Id == answer.ProblemId);

                if (problem == null)
                {
                    continue;
                }

                var isCorrect = string.Equals(
                    answer.StudentAnswer.Trim(),
                    problem.CorrectAns.Trim(),
                    StringComparison.OrdinalIgnoreCase);

                if (isCorrect)
                {
                    correctAnswers++;
                }

                var submission = new AnswerSubmission
                {
                    ProblemId = problem.Id,
                    StudentAnswer = answer.StudentAnswer,
                    IsCorrect = isCorrect,
                    AIExplanation = problem.Explanation
                };

                submissions.Add(submission);

                reviews.Add(new AnswerReviewDto
                {
                    ProblemId = problem.Id,
                    Question = problem.QuestionText,
                    StudentAnswer = answer.StudentAnswer,
                    CorrectAnswer = problem.CorrectAns,
                    IsCorrect = isCorrect,
                    AIExplanation = problem.Explanation
                });
            }

                var totalQuestions = dto.Answers.Count;

                var scorePercentage = totalQuestions == 0
                    ? 0
                    : (double)correctAnswers / totalQuestions * 100;

                var result = new PracticeResultDto
                {
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers,
                    ScorePercentage = scorePercentage,
                    Reviews = reviews
                };
            
                return ApiResponse<PracticeResultDto>.SuccessResponse(result,"Practice submitted successfully");
            }


        public async Task<ApiResponse<List<PracticeHistoryDto>>> GetHistoryAsync(Guid studentId)
        {
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
            if(student == null)
            {
                return ApiResponse<List<PracticeHistoryDto>>.ErrorResponse(new List<string> { "Student Not Found" }, "Request Failed");
            }
            var history = await _unitOfWork.Students.GetStudentHistory(studentId);

            var dto = history.Select(attempt => new PracticeHistoryDto
            {
                Id = attempt.Id,
                Topic = attempt.AnswerSubmissions
                               .Select(x => x.Problem.Topic.Name)
                               .FirstOrDefault() ?? string.Empty,
                Score = attempt.Score,
                CompletedAt = attempt.SubmittedAt!.Value
            }).ToList();

            return ApiResponse<List<PracticeHistoryDto>>.SuccessResponse(dto, "Practice history retrieved successfully");
        }
    }
}
