using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.AI;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.AI;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GramaMaster.Application.Services
{
    public class AiService : IAIService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<GenerateQuestionsDto> _generateQuestionsValidator;

        public AiService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IValidator<GenerateQuestionsDto>? generateQuestionsValidator = null)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _generateQuestionsValidator = generateQuestionsValidator ?? new GenerateQuestionsValidator();
        }

        public async Task<ApiResponse<List<AIQuestionDto>>> GenerateQuestionsAsync(GenerateQuestionsDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<List<AIQuestionDto>>.ErrorResponse(new[] { "Payload cannot be empty." }, "Invalid Request");
            }

            var validationResult = await _generateQuestionsValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<List<AIQuestionDto>>.ErrorResponse(validationResult.Errors.Select(x => x.ErrorMessage), "Validation Failed");
            }

            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);
            if (topic == null)
            {
                return ApiResponse<List<AIQuestionDto>>.ErrorResponse(new[] { "Topic not found." }, "Request Failed");
            }

            var curriculum = await _unitOfWork.Curriculums.GetByIdAsync(dto.CurriculumId);
            if (curriculum == null)
            {
                return ApiResponse<List<AIQuestionDto>>.ErrorResponse(new[] { "Curriculum not found." }, "Request Failed");
            }

            var rules = await _unitOfWork.GrammarRules.FindAsync(r => r.TopicId == dto.TopicId);

            var count = Math.Clamp(dto.Count, 1, 20);
            var generatedQuestions = new List<AIQuestionDto>();

            // Generate structured grammar questions based on topic and rules
            for (int i = 1; i <= count; i++)
            {
                var ruleContext = rules.Count > 0 ? rules[(i - 1) % rules.Count] : null;
                var question = CreateGrammarQuestion(topic.Name, ruleContext, dto.Difficulty, dto.ProblemType, i);
                generatedQuestions.Add(question);
            }

            if (dto.SaveToDatabase)
            {
                // Find a system or admin user to attribute question creation
                var adminUser = (await _unitOfWork.Users.FindAsync(u => u.Role == UserRole.Admin)).FirstOrDefault()
                                ?? (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault();

                var creatorId = adminUser?.Id ?? Guid.Empty;

                foreach (var q in generatedQuestions)
                {
                    var problem = new Problem
                    {
                        Id = Guid.NewGuid(),
                        TopicId = dto.TopicId,
                        CreatedByUserId = creatorId,
                        ProblemType = q.ProblemType,
                        QuestionText = q.QuestionText,
                        CorrectAns = q.CorrectAnswer,
                        Explanation = q.Explanation,
                        Difficulty = q.Difficulty,
                        IsAiGenerated = true
                    };

                    foreach (var optText in q.Options)
                    {
                        problem.ProblemOptions.Add(new ProblemOptions
                        {
                            Id = Guid.NewGuid(),
                            ProblemId = problem.Id,
                            OptionText = optText,
                            IsCorrect = string.Equals(optText.Trim(), q.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase)
                        });
                    }

                    await _unitOfWork.Problems.AddAsync(problem);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            return ApiResponse<List<AIQuestionDto>>.SuccessResponse(generatedQuestions, "Questions generated successfully.");
        }

        public async Task<ApiResponse<AIExplanationDto>> ExplainAnswerAsync(Guid answerSubmissionId)
        {
            var submission = await _unitOfWork.Exams.GetAnswerSubmissionByIdAsync(answerSubmissionId);
            if (submission == null)
            {
                return ApiResponse<AIExplanationDto>.ErrorResponse(new[] { "Answer submission not found." }, "Request Failed");
            }

            var problem = submission.Problem;
            var isCorrect = submission.IsCorrect;

            string explanationText;
            if (!string.IsNullOrWhiteSpace(submission.AIExplanation))
            {
                explanationText = submission.AIExplanation;
            }
            else
            {
                var topicName = problem.Topic?.Name ?? "English Grammar";
                explanationText = isCorrect
                    ? $"Correct! Your answer '{submission.StudentAnswer}' accurately applies the rules of {topicName}. {problem.Explanation}"
                    : $"Incorrect. You selected '{submission.StudentAnswer}', but the correct answer is '{problem.CorrectAns}'. Rule Context: In {topicName}, {problem.Explanation}";

                submission.AIExplanation = explanationText;
                await _unitOfWork.SaveChangesAsync();
            }

            var result = new AIExplanationDto
            {
                ProblemId = submission.ProblemId,
                StudentAnswer = submission.StudentAnswer,
                CorrectAnswer = problem.CorrectAns,
                Explanation = explanationText
            };

            return ApiResponse<AIExplanationDto>.SuccessResponse(result, "Explanation generated successfully.");
        }

        public async Task<ApiResponse<List<WeakTopicDto>>> AnalyzeWeakTopicsAsync(Guid studentId)
        {
            var attempts = await _unitOfWork.Students.GetStudentAttemptsWithAnswersAsync(studentId);
            if (attempts == null || !attempts.Any())
            {
                return ApiResponse<List<WeakTopicDto>>.SuccessResponse(new List<WeakTopicDto>(), "No attempt history found.");
            }

            var submissions = attempts
                .SelectMany(a => a.AnswerSubmissions)
                .Where(s => !s.IsDeleted && s.Problem != null)
                .ToList();

            if (!submissions.Any())
            {
                return ApiResponse<List<WeakTopicDto>>.SuccessResponse(new List<WeakTopicDto>(), "No answer submissions available for analysis.");
            }

            var weakTopics = submissions
                .GroupBy(s => new { s.Problem.TopicId, TopicName = s.Problem.Topic?.Name ?? "Grammar Topic" })
                .Select(g =>
                {
                    var total = g.Count();
                    var correct = g.Count(s => s.IsCorrect);
                    var accuracy = total > 0 ? Math.Round((double)correct / total * 100, 2) : 0.0;

                    return new WeakTopicDto
                    {
                        TopicId = g.Key.TopicId,
                        TopicName = g.Key.TopicName,
                        TotalAttempts = total,
                        Accuracy = accuracy
                    };
                })
                .Where(t => t.Accuracy < 70.0 || t.TotalAttempts < 3)
                .OrderBy(t => t.Accuracy)
                .ThenByDescending(t => t.TotalAttempts)
                .ToList();

            return ApiResponse<List<WeakTopicDto>>.SuccessResponse(weakTopics, "Weak topics analyzed successfully.");
        }

        public async Task<ApiResponse<List<TopicRecommendationDto>>> RecommendTopicsAsync(Guid studentId)
        {
            var weakTopicsResponse = await AnalyzeWeakTopicsAsync(studentId);
            var weakTopics = weakTopicsResponse.Data ?? new List<WeakTopicDto>();

            var recommendations = new List<TopicRecommendationDto>();

            if (weakTopics.Any())
            {
                var primaryWeak = weakTopics.First();
                recommendations.Add(new TopicRecommendationDto
                {
                    Title = $"Priority Focus: {primaryWeak.TopicName}",
                    Recommendation = $"Your current accuracy in '{primaryWeak.TopicName}' is {primaryWeak.Accuracy}%. We recommend reviewing key grammar rules and completing 10 practice problems before attempting your next contest.",
                    WeakTopics = weakTopics.Take(3).ToList()
                });

                if (weakTopics.Count > 1)
                {
                    var secondaryWeak = weakTopics.Skip(1).Take(2).ToList();
                    recommendations.Add(new TopicRecommendationDto
                    {
                        Title = "Secondary Reinforcement",
                        Recommendation = $"Strengthen your foundational knowledge in {string.Join(" and ", secondaryWeak.Select(w => w.TopicName))}.",
                        WeakTopics = secondaryWeak
                    });
                }
            }
            else
            {
                var allTopics = await _unitOfWork.Topics.GetAllAsync();
                recommendations.Add(new TopicRecommendationDto
                {
                    Title = "Comprehensive Mastery",
                    Recommendation = "Great job! You have consistent performance across tested topics. Try advancing to higher difficulty levels and joining upcoming global contests.",
                    WeakTopics = new List<WeakTopicDto>()
                });
            }

            return ApiResponse<List<TopicRecommendationDto>>.SuccessResponse(recommendations, "Recommendations generated successfully.");
        }

        private static AIQuestionDto CreateGrammarQuestion(
            string topicName,
            GrammarRule? rule,
            DifficultyType difficulty,
            ProblemType problemType,
            int index)
        {
            var ruleTitle = rule?.Title ?? $"{topicName} Rule #{index}";
            var ruleDesc = rule?.Content ?? "Master this fundamental rule of English grammar.";

            return new AIQuestionDto
            {
                QuestionText = $"Identify the correct grammar usage for {topicName} ({ruleTitle}): Which of the following sentences is grammatically sound?",
                Options = new List<string>
                {
                    $"Option A: Correct application aligned with {ruleTitle}.",
                    $"Option B: Incorrect tense consistency in {topicName}.",
                    $"Option C: Misplaced modifier or agreement error.",
                    $"Option D: Non-standard auxiliary verb placement."
                },
                CorrectAnswer = $"Option A: Correct application aligned with {ruleTitle}.",
                Explanation = $"Based on {ruleTitle}: {ruleDesc}",
                Difficulty = difficulty,
                ProblemType = problemType
            };
        }
    }
}
