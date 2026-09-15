using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Security;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Student;


namespace GramaMaster.Application.Services
{
    public class StudentService:IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<UpdateStudentProfileDto> _updateStudentProfileValidator;

        public StudentService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateStudentProfileDto>? updateStudentProfileValidator = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _updateStudentProfileValidator = updateStudentProfileValidator ?? new UpdateStudentProfileValidator();
        }

        private async Task<List<StudentTopicProgressDto>> GetTopicProgressInternalAsync(Guid studentId, Guid curriculumId)
        {
            var topics = await _unitOfWork.Students.GetCurriculumTopicsWithProblemsAsync(curriculumId);
            var attempts = await _unitOfWork.Students.GetStudentAttemptsWithAnswersAsync(studentId);

            var answersByTopic = attempts
                .SelectMany(a => a.AnswerSubmissions)
                .Where(ans => !ans.IsDeleted && ans.Problem!=null)
                .GroupBy(ans => ans.Problem.TopicId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<StudentTopicProgressDto>();

                foreach (var topic in topics)
                {
                    answersByTopic.TryGetValue(topic.Id, out var answers);
                    var totalSolved = answers?.Count ?? 0;
                    var correctCount = answers?.Count(a => a.IsCorrect) ?? 0;
                    var wrongCount = totalSolved - correctCount;
                    var accuracy = totalSolved > 0 ? Math.Round((double)correctCount / totalSolved * 100, 2) : 0.0;
                    var totalProblems = topic.Problems.Count(p => !p.IsDeleted);
                    var progressPercentage = totalProblems > 0
                        ? Math.Min(100.0, Math.Round((double)totalSolved / totalProblems * 100, 2))
                        : 0.0;
                    var recommendation = accuracy switch
                    {
                        >= 80 when totalSolved >= 5 => "Hard",
                        >= 50 => "Medium",
                        > 0 => "Easy",
                        _ => "Beginner"
                    };
                result.Add(new StudentTopicProgressDto
                {
                    TopicId = topic.Id,
                    TopicName = topic.Name,
                    TotalSolved = totalSolved,
                    CorrectAnswers = correctCount,
                    WrongAnswers = wrongCount,
                    Accuracy = accuracy,
                    ProgressPercentage = progressPercentage,
                    DifficultyRecommendation = recommendation
                });
            }
            return result;
        }
        public async Task<ApiResponse<StudentProfileDto>> GetProfileAsync(Guid studentId)
        {
            var std = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if(std == null)
            {
                return ApiResponse<StudentProfileDto>.ErrorResponse(new[] {"Student Profile not found"},"Not found");
            }
           

            var student = new StudentProfileDto
            {
                Id = std.Id,
                FullName = std.User?.FullName??string.Empty,
                Email = std.User?.Email??string.Empty,
                ProfileImageUrl = std.ProfileImageUrl,
                InstitutionName = std.InstitutionName,
                Curriculum = std.CurriculumType,
                Group = std.Group,
                ExamBatchYear = std.ExamBatchYear,
                Board = std.Board
            };
            return ApiResponse<StudentProfileDto>.SuccessResponse(student,"student profile fetched successfully");
        }

        public async Task<ApiResponse<bool>> UpdateProfileAsync(Guid studentId,UpdateStudentProfileDto dto)
        {
            var std = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if(std == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] {"Student Profile not found"},"Not found");
            }
            if(dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Request playload can not be empty" }, "Inavalid Request");
            }
            var validationResult = await _updateStudentProfileValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) 
            {
                return ApiResponse<bool>.ErrorResponse(validationResult.Errors.Select(e => e.ErrorMessage), "Validation Failed");   
            }
            std.ProfileImageUrl = dto.ProfileImageUrl?.Trim();
            std.InstitutionName = dto.InstitutionName.Trim();
            std.Group = dto.Group;
            std.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Students.Update(std);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Student profile updated successfully");
        }

        public async Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(Guid studentId)
        {
            var std = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if (std == null)
            {
                return ApiResponse<StudentDashboardDto>.ErrorResponse(new[] { "Student Profile not found" }, "Not found");
            }

            var topicProgress = await GetTopicProgressInternalAsync(std.Id, std.CurriculumId);

            var dashboard = new StudentDashboardDto
            {
                TotalPracticeQuestions = await _unitOfWork.Problems.GetPracticeProbCountByStudentIdAsync(std.Id),
                TotalContestsJoined = await _unitOfWork.Contests.GetTotalContestJoinedByUserCountAsync(std.Id),
                TotalContestsCompleted = await _unitOfWork.Contests.GetTotalContestCompletedByUserCountAsync(std.Id),
                OverallAccuracy = await _unitOfWork.Problems.GetOverallAccuracyByStudentIdAsync(std.Id),
                CurrentRank = 0,
                WeakTopics = topicProgress.Where(t => t.TotalSolved>0 && t.Accuracy<50).OrderBy(t => t.Accuracy).Take(5).ToList(),
                StrongTopics = topicProgress.Where(t => t.TotalSolved>0 && t.Accuracy>=70).OrderByDescending(t => t.Accuracy).Take(5).ToList()
            };

            return ApiResponse<StudentDashboardDto>.SuccessResponse(dashboard, "Dashboard data fetched successfully");
        }

        public async Task<ApiResponse<StudentAnalyticsDto>> GetAnalyticsAsync(Guid studentId)
        {
            var std = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if (std == null)
            {
                return ApiResponse<StudentAnalyticsDto>.ErrorResponse(new[] { "Student Profile not found" }, "Not found");
            }
            var attempts = await _unitOfWork.Students.GetStudentAttemptsWithAnswersAsync(std.Id);
            var allAnswers = attempts.SelectMany(x => x.AnswerSubmissions).Where(a => !a.IsDeleted).ToList();

            var totalQuestions = allAnswers.Count;
            var correctAnswers = allAnswers.Count(x => x.IsCorrect);
            var wrongAnswers = totalQuestions - correctAnswers;
            var accuracy = totalQuestions > 0 ? Math.Round((double)correctAnswers / totalQuestions * 100, 2) : 0.0;

            var completedAttemps = attempts.Where(a => a.SubmittedAt.HasValue).ToList();
            var totalSeconds = completedAttemps.Sum(a => (a.SubmittedAt!.Value - a.StartedAt).TotalSeconds);
            var avgSeconds = totalQuestions>0 && totalSeconds>0 ? totalSeconds / totalQuestions : 0;

            var analytics = new StudentAnalyticsDto
            {
                TotalQuestionsSolved = totalQuestions,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                Accuracy = accuracy,
                AverageTimePerQuestion = TimeSpan.FromSeconds(Math.Max(0,avgSeconds))
            };

            return ApiResponse<StudentAnalyticsDto>.SuccessResponse(analytics, "Student analytics fetched successfully");

        }

        public async Task<ApiResponse<StudentProgressDto>> GetProgressAsync(Guid studentId)
        {
            var std = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if (std == null)
            {
                return ApiResponse<StudentProgressDto>.ErrorResponse(new[] { "Student Profile not found" }, "Not found");
            }

            var topicProgress = await GetTopicProgressInternalAsync(std.Id,std.CurriculumId);
            var totalTopics = topicProgress.Count;
            var completedTopics = topicProgress.Count(t => t.TotalSolved > 5 && t.Accuracy >= 70);

            var progress = new StudentProgressDto
            {
                TotalTopics = totalTopics,
                CompletedTopics = completedTopics,
                ProgressPercentage = totalTopics > 0 ? Math.Round((double)completedTopics / totalTopics * 100, 2) : 0.0,
                Topics = topicProgress
            };
            return ApiResponse<StudentProgressDto>.SuccessResponse(progress, "student progress fetched successfully");
        }

        public async Task<ApiResponse<List<StudentTopicProgressDto>>> GetTopicProgressAsync(Guid studentId)
        {
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);

            if(student == null)
            {
                return ApiResponse<List<StudentTopicProgressDto>>.ErrorResponse(new[] { "Student Profile not found" }, "Not found");
            }
            var topicProgress = await GetTopicProgressInternalAsync(student.Id, student.CurriculumId);

            return ApiResponse<List<StudentTopicProgressDto>>.SuccessResponse(topicProgress, "student topic progress fetched successfully");


        }
    }
}
