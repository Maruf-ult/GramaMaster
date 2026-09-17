using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;
using GramaMaster.Application.DTOs.Teacher;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Teacher;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class TeacherService:ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTeacherProfileDto> _updateTeacherProfileValidator;

        public TeacherService(IUnitOfWork unitOfWork,IValidator<UpdateTeacherProfileDto>? updateTeacherProfileValidator=null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _updateTeacherProfileValidator = updateTeacherProfileValidator ?? new UpdateTeacherProfileValidator();
        }
        public async Task<ApiResponse<TeacherProfileDto>> GetProfileAsync(Guid teacherId)
        {
            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);

            if(teacher == null)
            {
                return ApiResponse<TeacherProfileDto>.ErrorResponse(new List<string> { "Teacher not found" }, "Request Failed");
            }

            var teacherProfile = new TeacherProfileDto
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                FullName = teacher.User.FullName,
                Email = teacher.User.Email,
                ProfileImageUrl = teacher.ProfileImageUrl?.Trim(),
                SscResult = teacher.SscResult,
                SscGroup = teacher.SscGroup,
                SscBoard = teacher.SscBoard,
                HscResult = teacher.HscResult,
                HscGroup = teacher.HscGroup,
                HscBoard = teacher.HscBoard,
                Department = teacher.Department,
                University = teacher.University
            };

            return ApiResponse<TeacherProfileDto>.SuccessResponse(teacherProfile, "Profile retrieved successfully");


        }

        public async Task<ApiResponse<bool>> UpdateProfileAsync(Guid teacherId, UpdateTeacherProfileDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload Cant be empty" }, "Invalid Request");
            }

            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);

            if (teacher == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Teacher not found" }, "Request Failed");
            }

            var validationResult = await _updateTeacherProfileValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<bool>.ErrorResponse(errors, "Validation Failed");
            }

            teacher.ProfileImageUrl = dto.ProfileImageUrl?.Trim();
            teacher.Department = dto.Department;
            teacher.University = dto.University;
            teacher.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Teachers.Update(teacher);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Profile updated successfully");
        }

        public async Task<ApiResponse<TeacherDashboardDto>> GetDashboardAsync(Guid teacherId)
        {
            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);

            if (teacher == null)
            {
                return ApiResponse<TeacherDashboardDto>.ErrorResponse(new List<string> { "Teacher not found" }, "Request Failed");
            }

            var totalTeams = teacher.Teams.Count;
            var totalStudents = teacher.Teams.Sum(t => t.MaxStudents);
            var totalContests = await _unitOfWork.Teachers.GetTeacherContestCountAsync(teacher.Id);
            var PracticeProblems = await _unitOfWork.Problems.GetTeacherProblemsAsync(teacher.Id);
            var totalPracticeProblems = PracticeProblems.Count();
            var aiGeneratedProblems = PracticeProblems.Count(p => p.IsAiGenerated);
            var activeContests = await _unitOfWork.Contests.GetRunningContestsByTeacherAsync(teacher.Id);
            var totalActiveContests = activeContests.Count();
            var allTeams = await _unitOfWork.Teams.GetAllAsync();
            var tt = allTeams.Where(x => x.TeacherId == teacher.Id && !x.IsDeleted && x.IsActive).ToList();
            int sum = 0;
            foreach(var team in tt)
            {
                var kk = await _unitOfWork.Problems.GetOverrallAccuracyOfTeamMembersAsync(team.Id);
                sum += kk;
            }
            var recentContests = activeContests.Select(c => new RecentContestDto
            {
                ContestId = c.Id,
                Title = c.Title,
                StartAt = c.CreatedAt,
                EndAt = c.EndAt,
                ParticipantCount = c.ExamAttempts.Count
            }).Take(5).ToList();
            var teams = teacher.Teams.Select(t => new TeamSummaryDto
            {
                TeamId = t.Id,
                TeamName = t.Name,
                StudentCount = t.MaxStudents
            }).Take(5).ToList();

            return ApiResponse<TeacherDashboardDto>.SuccessResponse(new TeacherDashboardDto
            {
                TotalTeams = totalTeams,
                TotalStudents = totalStudents,
                TotalContests = totalContests,
                TotalPracticeProblems = totalPracticeProblems,
                AiGeneratedProblems = aiGeneratedProblems,
                ActiveContests = totalActiveContests,
                AverageStudentAccuracy = tt.Count > 0 ? (double)sum / tt.Count : 0.0,
                RecentContests = recentContests,
                Teams = teams
            }, "Dashboard retrieved successfully");

        }

        public async Task<ApiResponse<TeacherAnalyticsDto>> GetAnalyticsAsync(Guid teacherId)
        {
            var teacher = await _unitOfWork.Teachers.GetTeacherWithDetailsAsync(teacherId);

            if (teacher == null)
            {
                return ApiResponse<TeacherAnalyticsDto>.ErrorResponse(
                    new List<string> { "Teacher not found" },
                    "Request Failed");
            }

            var totalStudents = teacher.Teams
                .Where(t => !t.IsDeleted && t.IsActive)
                .Sum(t => t.MaxStudents);

            var problemsCreated =
                await _unitOfWork.Problems.GetTeacherProblemsAsync(teacher.Id);

            var totalProblemsCreated =
                await _unitOfWork.Teachers.GetTotalProblemsCreatedAsync(teacher.Id);

            var aiGeneratedProblems =
                problemsCreated.Count(p => p.IsAiGenerated);

            var manualProblems =
                problemsCreated.Count(p => !p.IsAiGenerated);

            var totalContest =
                await _unitOfWork.Teachers.GetTeacherContestCountAsync(teacher.Id);

            var runningContest =
                await _unitOfWork.Teachers.GetRunningContestCountAsync(teacher.Id);

            var completedContest =
                await _unitOfWork.Teachers.GetCompletedContestCountAsync(teacher.Id);

            var averageContestScore =
                await _unitOfWork.Contests.GetAverageContestScoreByTeacherId(
                    teacher.Id);

            var averageStudentAccuracy =
                await _unitOfWork.Problems.GetAverageStudentAccuracyByTeacherIdAsync(
                    teacher.Id);



            var topicPerformances = new List<TopicPerformanceDto>();

            var studentIds = teacher.Teams
                .Where(t => !t.IsDeleted && t.IsActive)
                .SelectMany(t => t.TeamMembers)
                .Where(tm => !tm.IsDeleted)
                .Select(tm => tm.StudentId)
                .Distinct()
                .ToList();


            var topicAccuracyData = new Dictionary<Guid, List<double>>();


            foreach (var studentId in studentIds)
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);

                if (student == null)
                    continue;


                var studentTopicProgress =
                    await GetTopicProgressInternalAsync(
                        studentId,
                        student.CurriculumId);


                foreach (var topic in studentTopicProgress)
                {
                    var existingTopic = topicPerformances
                        .FirstOrDefault(x => x.TopicId == topic.TopicId);


                    // Create topic entry if it doesn't exist
                    if (existingTopic == null)
                    {
                        existingTopic = new TopicPerformanceDto
                        {
                            TopicId = topic.TopicId,
                            TopicName = topic.TopicName,
                            QuestionsSolved = 0,
                            AverageAccuracy = 0,
                            WeakStudents = 0,
                            StrongStudents = 0
                        };

                        topicPerformances.Add(existingTopic);
                    }


                    existingTopic.QuestionsSolved += topic.TotalSolved;

                    if (topic.TotalSolved > 0)
                    {
                        if (!topicAccuracyData.ContainsKey(topic.TopicId))
                        {
                            topicAccuracyData[topic.TopicId] = new List<double>();
                        }

                        topicAccuracyData[topic.TopicId]
                            .Add(topic.Accuracy);


                        if (topic.Accuracy < 50)
                        {
                            existingTopic.WeakStudents++;
                        }


                        if (topic.Accuracy >= 80)
                        {
                            existingTopic.StrongStudents++;
                        }
                    }
                }
            }

            foreach (var topic in topicPerformances)
            {
                if (topicAccuracyData.TryGetValue(
                        topic.TopicId,
                        out var accuracies)
                    && accuracies.Count > 0)
                {
                    topic.AverageAccuracy =
                        Math.Round(accuracies.Average(), 2);
                }
                else
                {
                    topic.AverageAccuracy = 0;
                }
            }


            var analytics = new TeacherAnalyticsDto
            {
                TotalStudents = totalStudents,

                TotalProblemsCreated = totalProblemsCreated,

                AiGeneratedProblems = aiGeneratedProblems,

                ManualProblems = manualProblems,

                TotalContests = totalContest,

                RunningContests = runningContest,

                CompletedContests = completedContest,

                AverageContestScore = averageContestScore,

                AverageStudentAccuracy = averageStudentAccuracy,

                TopicPerformances = topicPerformances
            };


            return ApiResponse<TeacherAnalyticsDto>.SuccessResponse( analytics,"Teacher analytics retrieved successfully");
        }

        private async Task<List<StudentTopicProgressDto>> GetTopicProgressInternalAsync(Guid studentId, Guid curriculumId)
        {
            var topics = await _unitOfWork.Students.GetCurriculumTopicsWithProblemsAsync(curriculumId);
            var attempts = await _unitOfWork.Students.GetStudentAttemptsWithAnswersAsync(studentId);

            var answersByTopic = attempts
                .SelectMany(a => a.AnswerSubmissions)
                .Where(ans => !ans.IsDeleted && ans.Problem != null)
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


    }
}
