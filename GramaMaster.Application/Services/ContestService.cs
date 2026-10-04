using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Contests;
using GramaMaster.Application.DTOs.Topics;
using GramaMaster.Application.Exceptions;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Contests;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GramaMaster.Application.Services
{
    public class ContestService : IContestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateContestDto> _createContestValidator;
        private readonly IValidator<UpdateContestDto> _updateContestValidator;

        public ContestService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IValidator<CreateContestDto>? createContestValidator = null,
            IValidator<UpdateContestDto>? updateContestValidator = null)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _createContestValidator = createContestValidator ?? new CreateContestValidator();
            _updateContestValidator = updateContestValidator ?? new UpdateContestValidator();
        }

        public async Task<ApiResponse<ContestDto>> CreateContestAsync(Guid userId, CreateContestDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<ContestDto>.ErrorResponse(new[] { "Payload cannot be empty." }, "Invalid Request");
            }

            var validationResult = await _createContestValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<ContestDto>.ErrorResponse(validationResult.Errors.Select(x => x.ErrorMessage), "Validation Failed");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<ContestDto>.ErrorResponse(new[] { "User not found." }, "Request Failed");
            }

            var curriculum = await _unitOfWork.Curriculums.GetByIdAsync(dto.CurriculumId);
            if (curriculum == null)
            {
                return ApiResponse<ContestDto>.ErrorResponse(new[] { "Curriculum not found." }, "Request Failed");
            }

            if (dto.ContestType == ContestType.Local)
            {
                if (!dto.TeamId.HasValue)
                {
                    return ApiResponse<ContestDto>.ErrorResponse(new[] { "Team ID is required for local contest." }, "Request Failed");
                }

                var team = await _unitOfWork.Teams.GetByIdAsync(dto.TeamId.Value);
                if (team == null)
                {
                    return ApiResponse<ContestDto>.ErrorResponse(new[] { "Team not found." }, "Request Failed");
                }
            }

            var contest = new Contest
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                ContestType = dto.ContestType,
                CurriculumId = dto.CurriculumId,
                CreatedByUserId = userId,
                TeamId = dto.ContestType == ContestType.Local ? dto.TeamId : null,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                DurationMinutes = dto.DurationMinutes
            };

            await _unitOfWork.Contests.AddAsync(contest);

            if (dto.ProblemIds != null && dto.ProblemIds.Any())
            {
                var problems = await _unitOfWork.Problems.FindAsync(p => dto.ProblemIds.Contains(p.Id));
                foreach (var problem in problems)
                {
                    problem.ContestId = contest.Id;
                    await _unitOfWork.Problems.AddAsync(problem);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            var createdContest = await _unitOfWork.Contests.GetContestWithProblemsAsync(contest.Id);
            var result = new ContestDto
            {
                Id = contest.Id,
                Title = contest.Title,
                Description = contest.Description,
                ContestType = contest.ContestType,
                Curriculum = curriculum.Name,
                CreatedBy = user,
                TeamId = contest.TeamId,
                TeamName = contest.Team?.Name,
                StartAt = contest.StartAt,
                EndAt = contest.EndAt,
                DurationMinutes = contest.DurationMinutes,
                TotalProblems = createdContest?.Problems?.Count ?? dto.ProblemIds?.Count ?? 0,
                TotalParticipants = 0
            };

            return ApiResponse<ContestDto>.SuccessResponse(result, "Contest created successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateContestAsync(Guid contestId, UpdateContestDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload cannot be empty." }, "Invalid Request");
            }

            var validationResult = await _updateContestValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<bool>.ErrorResponse(validationResult.Errors.Select(x => x.ErrorMessage), "Validation Failed");
            }

            var contest = await _unitOfWork.Contests.GetContestWithProblemsAsync(contestId);
            if (contest == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Contest not found." }, "Request Failed");
            }

            var now = DateTime.UtcNow;
            if (contest.StartAt <= now && contest.EndAt >= now)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Cannot modify a contest that is currently running." }, "Request Failed");
            }

            contest.Title = dto.Title.Trim();
            contest.Description = dto.Description.Trim();
            contest.StartAt = dto.StartAt;
            contest.EndAt = dto.EndAt;
            contest.DurationMinutes = dto.DurationMinutes;

            // Update problems association
            var currentProblems = contest.Problems.ToList();
            var targetProblemIds = dto.ProblemIds ?? new List<Guid>();

            // Detach unselected problems
            foreach (var problem in currentProblems.Where(p => !targetProblemIds.Contains(p.Id)))
            {
                problem.ContestId = null;
                _unitOfWork.Problems.AddAsync(problem);
            }

            // Attach newly selected problems
            var currentProblemIds = currentProblems.Select(p => p.Id).ToHashSet();
            var newlyAddedIds = targetProblemIds.Where(id => !currentProblemIds.Contains(id)).ToList();
            if (newlyAddedIds.Any())
            {
                var newProblems = await _unitOfWork.Problems.FindAsync(p => newlyAddedIds.Contains(p.Id));
                foreach (var problem in newProblems)
                {
                    problem.ContestId = contest.Id;
                    _unitOfWork.Problems.AddAsync(problem);
                }
            }

            _unitOfWork.Contests.Update(contest);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Contest updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteContestAsync(Guid contestId)
        {
            var contest = await _unitOfWork.Contests.GetByIdAsync(contestId);
            if (contest == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Contest not found." }, "Request Failed");
            }

            var isRunning = await _unitOfWork.Contests.IsContestRunningAsync(contestId);
            if (isRunning)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Cannot delete an ongoing contest." }, "Request Failed");
            }

            // Detach problems before deletion
            var problems = await _unitOfWork.Contests.GetContestProblemsAsync(contestId);
            foreach (var problem in problems)
            {
                problem.ContestId = null;
                await _unitOfWork.Problems.AddAsync(problem);
            }

            _unitOfWork.Contests.Delete(contest);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Contest deleted successfully.");
        }

        public async Task<ApiResponse<ContestDetailsDto>> GetContestDetailsAsync(Guid contestId)
        {
            var contest = await _unitOfWork.Contests.GetContestWithProblemsAsync(contestId);
            if (contest == null)
            {
                throw new NotFoundException($"Contest with ID '{contestId}' was not found.");
            }

            var now = DateTime.UtcNow;
            var dto =  new ContestDetailsDto
            {
                Id = contest.Id,
                Title = contest.Title,
                Description = contest.Description,
                Curriculum = contest.Curriculum?.Name ?? string.Empty,
                CreatedBy = contest.CreatedByUser?.FullName ?? string.Empty,
                TeamName = contest.Team?.Name,
                StartAt = contest.StartAt,
                EndAt = contest.EndAt,
                DurationMinutes = contest.DurationMinutes,
                TotalProblems = contest.Problems?.Count(p => !p.IsDeleted) ?? 0,
                IsRunning = contest.StartAt <= now && contest.EndAt >= now,
                HasJoined = false,
                HasSubmitted = false
            };
            return ApiResponse<ContestDetailsDto>.SuccessResponse(dto, "Contest details retrieved successfully.");
        }

        public async Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetGlobalContestsAsync(QueryDto query)
        {
            query ??= new QueryDto();
            var contests = await _unitOfWork.Contests.GetGlobalContestsAsync(query);
            var now = DateTime.UtcNow;

            var cards = new List<ContestCardDto>();
            foreach (var c in contests)
            {
                var participantCount = await _unitOfWork.Contests.GetContestParticipantCountAsync(c.Id);
                cards.Add(new ContestCardDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ContestType = c.ContestType,
                    Curriculum = c.Curriculum?.Name ?? string.Empty,
                    StartAt = c.StartAt,
                    DurationMinutes = c.DurationMinutes,
                    ParticipantCount = participantCount,
                    IsRunning = c.StartAt <= now && c.EndAt >= now,
                    HasJoined = false
                });
            }

            return ApiResponse<PagedResultDto<ContestCardDto>>.SuccessResponse(new PagedResultDto<ContestCardDto>
            {
                Items = cards,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = cards.Count
            }, "Global contests retrieved successfully.");
        }

        public async Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetTeacherContestsAsync(Guid teacherId, QueryDto query)
        {
            query ??= new QueryDto();
            var contests = await _unitOfWork.Contests.GetTeacherContestsAsync(teacherId, query);
            var now = DateTime.UtcNow;

            var cards = new List<ContestCardDto>();
            foreach (var c in contests)
            {
                var participantCount = await _unitOfWork.Contests.GetContestParticipantCountAsync(c.Id);
                cards.Add(new ContestCardDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ContestType = c.ContestType,
                    Curriculum = c.Curriculum?.Name ?? string.Empty,
                    StartAt = c.StartAt,
                    DurationMinutes = c.DurationMinutes,
                    ParticipantCount = participantCount,
                    IsRunning = c.StartAt <= now && c.EndAt >= now,
                    HasJoined = false
                });
            }

            return ApiResponse<PagedResultDto<ContestCardDto>>.SuccessResponse(new PagedResultDto<ContestCardDto>
            {
                Items = cards,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = cards.Count
            }, "Teacher contests retrieved successfully.");
        }

        public async Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetStudentAvailableContestsAsync(Guid studentId, QueryDto query)
        {
            query ??= new QueryDto();
            var contests = await _unitOfWork.Contests.GetStudentAvailableContestsAsync(studentId);
            var now = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contests = contests
                    .Where(c => c.Title.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                c.Description.Contains(query.Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var totalCount = contests.Count;
            var paged = contests
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var cards = new List<ContestCardDto>();
            foreach (var c in paged)
            {
                var participantCount = await _unitOfWork.Contests.GetContestParticipantCountAsync(c.Id);
                var hasJoined = await _unitOfWork.Contests.HasStudentJoinedAsync(c.Id, studentId);
                cards.Add(new ContestCardDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ContestType = c.ContestType,
                    Curriculum = c.Curriculum?.Name ?? string.Empty,
                    StartAt = c.StartAt,
                    DurationMinutes = c.DurationMinutes,
                    ParticipantCount = participantCount,
                    IsRunning = c.StartAt <= now && c.EndAt >= now,
                    HasJoined = hasJoined
                });
            }

            return ApiResponse<PagedResultDto<ContestCardDto>>.SuccessResponse(new PagedResultDto<ContestCardDto>
            {
                Items = cards,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            }, "Student available contests retrieved successfully.");
        }

        public async Task<ApiResponse<List<ContestLeaderBoardDto>>> GetLeaderboardAsync(Guid contestId)
        {
            var attempts = await _unitOfWork.Exams.GetLeaderboardAsync(contestId, new QueryDto { PageNumber = 1, PageSize = 100 });
            var leaderboard = new List<ContestLeaderBoardDto>();

            var rank = 1;
            foreach (var attempt in attempts.OrderByDescending(a => a.Score).ThenBy(a => a.SubmittedAt))
            {
                var completionTime = attempt.SubmittedAt.HasValue
                    ? attempt.SubmittedAt.Value - attempt.StartedAt
                    : TimeSpan.Zero;

                var correctAnswers = attempt.AnswerSubmissions?.Count(a => a.IsCorrect) ?? 0;

                leaderboard.Add(new ContestLeaderBoardDto
                {
                    Rank = rank++,
                    StudentId = attempt.StudentId,
                    StudentName = attempt.Student?.User?.FullName ?? "Anonymous Student",
                    ProfileImageUrl = attempt.Student?.ProfileImageUrl,
                    Score = attempt.Score,
                    CorrectAnswers = correctAnswers,
                    TotalQuestions = attempt.TotalQuestions > 0 ? attempt.TotalQuestions : (attempt.AnswerSubmissions?.Count ?? 0),
                    CompletionTime = completionTime
                });
            }

            return ApiResponse<List<ContestLeaderBoardDto>>.SuccessResponse(leaderboard, "Leaderboard retrieved successfully.");
        }

        public async Task<ApiResponse<ContestAnalyticsDto>> GetAnalyticsAsync(Guid contestId)
        {
            var contest = await _unitOfWork.Contests.GetContestWithAttemptsAsync(contestId);
            if (contest == null)
            {
                return ApiResponse<ContestAnalyticsDto>.ErrorResponse(new[] { "Contest not found." }, "Request Failed");
            }

            var attempts = contest.ExamAttempts.Where(a => !a.IsDeleted && a.SubmittedAt.HasValue).ToList();
            var totalParticipants = attempts.Count;
            var averageScore = totalParticipants > 0 ? Math.Round(attempts.Average(a => a.Score), 2) : 0.0;
            var highestScore = totalParticipants > 0 ? attempts.Max(a => a.Score) : 0.0;
            var lowestScore = totalParticipants > 0 ? attempts.Min(a => a.Score) : 0.0;
            var passRate = totalParticipants > 0
                ? Math.Round((double)attempts.Count(a => a.Score >= 50.0) / totalParticipants * 100, 2)
                : 0.0;

            var allSubmissions = attempts
                .SelectMany(a => a.AnswerSubmissions)
                .Where(s => !s.IsDeleted && s.Problem != null)
                .ToList();

            var topicAnalytics = allSubmissions
                .GroupBy(s => new { s.Problem.TopicId, TopicName = s.Problem.Topic?.Name ?? "General" })
                .Select(g =>
                {
                    var count = g.Count();
                    var correct = g.Count(s => s.IsCorrect);
                    var accuracy = count > 0 ? Math.Round((double)correct / count * 100, 2) : 0.0;
                    return new TopicAnalyticsDto
                    {
                        TopicId = g.Key.TopicId,
                        TopicName = g.Key.TopicName,
                        Accuracy = accuracy,
                        TotalAttempts = count
                    };
                })
                .ToList();

            var ndto = new ContestAnalyticsDto
            {
                ContestId = contest.Id,
                Title = contest.Title,
                TotalParticipants = totalParticipants,
                AverageScore = averageScore,
                HighestScore = highestScore,
                LowestScore = lowestScore,
                PassRate = passRate,
                Topics = topicAnalytics
            };
            return ApiResponse<ContestAnalyticsDto>.SuccessResponse(ndto, "Contest analytics retrieved successfully.");
        }
    }
}
