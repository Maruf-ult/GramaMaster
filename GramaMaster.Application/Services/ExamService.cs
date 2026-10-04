using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Exams;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Exams;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class ExamService:IExamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SubmitExamDto> _submitExamValidator;
        public ExamService(IMapper mapper,IUnitOfWork unitOfWork,IValidator<SubmitExamDto>submitExamValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _submitExamValidator = submitExamValidator ?? new SubmitExamValidator();
        }
        public async Task<ApiResponse<ExamAttemptDto>> StartExamAsync(  Guid studentId,StartExamDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "Payload cant be empty" }, "Request Failed");
            }
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
            if(student == null)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "Student Not Found" }, "Request Failed");
            }
            var contest = await _unitOfWork.Contests.GetContestWithProblemsAsync(dto.ContestId);
            if(contest == null)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "Contest Not Found" }, "Request Failed");
            }

            var now = DateTime.UtcNow;

            if (contest.StartAt > now)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "Contest has not started yet" },
                    "Request Failed");
            }
            if (contest.EndAt < now)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "Contest has already ended" },
                    "Request Failed");
            }

            var alreadyTaken = await _unitOfWork.Exams.HasStudentAlreadyTakenContestAsync(studentId, dto.ContestId);

            if (alreadyTaken)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new[] { "You have already started this contest" },
                    "Request Failed");
            }
            if (contest.Problems == null || !contest.Problems.Any())
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(
                    new[] { "This contest has no problems" },
                    "Request Failed");
            }

            var examAttempt = new ExamAttempt
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                ContestId = contest.Id,
                StartedAt = DateTime.UtcNow,
                SubmittedAt = null,
                Score = 0,
                TotalQuestions = contest.Problems.Count
            };

            await _unitOfWork.Exams.AddAsync(examAttempt);

            await _unitOfWork.SaveChangesAsync();

            ExamAttemptDto result = _mapper.Map<ExamAttemptDto>(examAttempt);

            return ApiResponse<ExamAttemptDto>.SuccessResponse(result, "Exam started successfully");

        }

        public async Task<ApiResponse<ExamResultDto>> SubmitExamAsync( Guid studentId,SubmitExamDto dto)
        {

            if(dto == null)
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(new[] { "Payload cant be empty" }, "Request Failed");
            }

            var validationResult = await _submitExamValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<ExamResultDto>.ErrorResponse(errors, "Validation Failed");
            }
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
            if(student == null)
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(new List<string> { "Student Not Found" }, "Request Failed");
            }
            var attempt = await _unitOfWork.Exams.GetAttemptWithAnswersAsync(dto.AttemptId);

            if (attempt == null)
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(
                    new[] { "Exam attempt not found" },
                    "Request Failed");
            }
            if(attempt.StudentId != studentId)
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(
               new[] { "This exam attempt does not belong to this student" },
               "Request Failed");
            }

            if (attempt.SubmittedAt != null)
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(
            new[] { "This exam has already been submitted" },
            "Request Failed");
            }
            if (attempt.AnswerSubmissions.Any())
            {
                return ApiResponse<ExamResultDto>.ErrorResponse(
                    new[] { "Answers have already been submitted for this attempt" },
                    "Request Failed");
            }

            var reviews = new List<AnswerReviewDto>();

            int correctAnswerCount = 0;

            foreach(var answer in dto.Answers)
            {
                var problem = attempt.Contest.Problems
                    .FirstOrDefault(x => x.Id == answer.ProblemId);
                
                if(problem == null)
                {
                return ApiResponse<ExamResultDto>.ErrorResponse(
                new[]
                {
                    $"Problem {answer.ProblemId} does not belong to this exam"
                },
                "Request Failed");
                }
                var isCorrect = string.Equals(
                    answer.StudentAnswer?.Trim(),
                    problem.CorrectAns?.Trim(),
                    StringComparison.OrdinalIgnoreCase);

                if (isCorrect)
                {
                    correctAnswerCount++;
                }
                var submission = new AnswerSubmission
                {
                    Id = Guid.NewGuid(),
                    ExamAttemptId = attempt.Id,
                    ProblemId = problem.Id,
                    StudentAnswer = answer.StudentAnswer,
                    IsCorrect = isCorrect,
                    AIExplanation = problem.Explanation
                };

                attempt.AnswerSubmissions.Add(submission);

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

            int totalQuestions = attempt.TotalQuestions;

            double percentage = totalQuestions > 0
                ? (double)correctAnswerCount / totalQuestions * 100
                : 0;

            double score = correctAnswerCount;

            attempt.Score = score;
            attempt.SubmittedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            var result = new ExamResultDto
            {
                AttemptId = attempt.Id,
                CorrectAnswers = correctAnswerCount,
                TotalQuestions = totalQuestions,
                Percentage = percentage,
                Score = score,
                Reviews = reviews
            };

            return ApiResponse<ExamResultDto>.SuccessResponse(
                result,
                "Exam submitted successfully");


        }

        public async Task<ApiResponse<ExamAttemptDto>> GetAttemptAsync(Guid studentId,Guid attemptId)
        {
            var attempt = await _unitOfWork.Exams.GetStudentAttemptAsync(studentId, attemptId);

            if(attempt == null)
            {
                return ApiResponse<ExamAttemptDto>.ErrorResponse(new List<string> { "Attempt not found" }, "Request Failed"); ;
            }
            ExamAttemptDto dto = _mapper.Map<ExamAttemptDto>(attempt);
            return ApiResponse<ExamAttemptDto>.SuccessResponse(dto, "Exam Attempt found");

        }

        public async Task<ApiResponse<List<LeaderboardDto>>> GetLeaderboardAsync(
            Guid contestId,
            QueryDto query)
        {
            var contest = await _unitOfWork.Contests.GetContestWithAttemptsAsync(contestId);

            if(contest == null)
            {
                return ApiResponse<List<LeaderboardDto>>.ErrorResponse(new List<string> { "Contest not found" }, "Request Failed");
            }
            var attempt = await _unitOfWork.Exams.GetLeaderboardAsync(contestId, query);
            return ApiResponse<List<LeaderboardDto>>.SuccessResponse(_mapper.Map<List<LeaderboardDto>>(attempt), "LeaderBoard fetched successfully");
        }

        public async Task<ApiResponse<List<AnswerReviewDto>>> ReviewAnswersAsync(Guid attemptId)
        {
            var ans = await _unitOfWork.Exams.GetAttemptWithAnswersAsync(attemptId);

            if(ans == null)
            {
                return ApiResponse<List<AnswerReviewDto>>.ErrorResponse(new[] { "Exam attempt not found" }, "Request Failed");
            }
            var reviews = ans.AnswerSubmissions
                .Where(x => !x.IsDeleted)
                .Select(x => new AnswerReviewDto
                {
                    ProblemId = x.ProblemId,
                    Question = x.Problem.QuestionText,
                    StudentAnswer = x.StudentAnswer,
                    CorrectAnswer = x.Problem.CorrectAns,
                    IsCorrect = x.IsCorrect,
                    AIExplanation = x.AIExplanation
                })
                .ToList();
            return ApiResponse<List<AnswerReviewDto>>.SuccessResponse(reviews, "Answer reviewed succesfully");
        }
    }
}
