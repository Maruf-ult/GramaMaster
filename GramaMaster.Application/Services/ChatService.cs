using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Chats;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Exceptions;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Chats;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GramaMaster.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ChatRequestDto> _chatRequestValidator;

        public ChatService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IValidator<ChatRequestDto>? chatRequestValidator = null)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _chatRequestValidator = chatRequestValidator ?? new ChatRequestValidator();
        }

        public async Task<ApiResponse<ChatResponseDto>> SendMessageAsync(Guid studentId, ChatRequestDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Chat request payload cannot be empty.");
            }

            var validationResult = await _chatRequestValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BadRequestException(error);
            }

            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new NotFoundException($"Student with ID '{studentId}' was not found.");
            }

            ChatSession? session = null;
            if (dto.SessionId != Guid.Empty)
            {
                session = await _unitOfWork.Chats.GetSessionWithMessagesAsync(dto.SessionId);
            }

            if (session == null)
            {
                session = new ChatSession
                {
                    Id = dto.SessionId != Guid.Empty ? dto.SessionId : Guid.NewGuid(),
                    StudentId = studentId,
                    StartedAt = DateTime.UtcNow
                };
                await _unitOfWork.Chats.AddSessionAsync(session);
            }

            var studentMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = session.Id,
                Sender = "Student",
                Content = dto.Message.Trim(),
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Chats.AddAsync(studentMessage);

            // Generate contextual grammar tutor response
            var aiReply = GenerateTutorReply(dto.Message);

            var aiMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = session.Id,
                Sender = "GramaMaster AI",
                Content = aiReply,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Chats.AddAsync(aiMessage);

            await _unitOfWork.SaveChangesAsync();

            var ndto = new ChatResponseDto
            {
                SessionId = session.Id,
                Response = aiReply,
                CreatedAt = aiMessage.CreatedAt
            };
            return ApiResponse<ChatResponseDto>.SuccessResponse(ndto, "Message sent and AI response generated successfully.");>
        }

        public async Task<ApiResponse<List<ChatHistoryDto>>> GetHistoryAsync(Guid studentId)
        {
            var sessions = await _unitOfWork.Chats.GetStudentSessionsAsync(studentId);
            var history = new List<ChatHistoryDto>();

            foreach (var session in sessions)
            {
                history.Add(new ChatHistoryDto
                {
                    SessionId = session.Id,
                    Messages = session.Messages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new ChatMessageDto
                        {
                            Sender = m.Sender,
                            Message = m.Content,
                            CreatedAt = m.CreatedAt
                        })
                        .ToList()
                });
            }

            return ApiResponse<List<ChatHistoryDto>>.SuccessResponse(history, "Chat history retrieved successfully.");
        }

        public async Task<ApiResponse<List<ChatMessageDto>>> GetSessionMessagesAsync(Guid sessionId)
        {
            var messages = await _unitOfWork.Chats.GetHistoryAsync(sessionId);
            return ApiResponse<List<ChatMessageDto>>.SuccessResponse(
                messages.Select(m => new ChatMessageDto
                {
                    Sender = m.Sender,
                    Message = m.Content,
                    CreatedAt = m.CreatedAt
                }).ToList(),
                "Session messages retrieved successfully."
            );
        }

        private static string GenerateTutorReply(string userPrompt)
        {
            var lower = userPrompt.ToLowerInvariant();

            if (lower.Contains("tense") || lower.Contains("past") || lower.Contains("present") || lower.Contains("future"))
            {
                return "In English grammar, tenses express the time of an action. Remember: Present Perfect (have/has + V3) connects past actions with present relevance, whereas Simple Past (V2) refers to an action completed at a definite time in the past.";
            }

            if (lower.Contains("voice") || lower.Contains("active") || lower.Contains("passive"))
            {
                return "Active vs. Passive voice: In Active voice, the subject performs the action (e.g., 'The teacher explained the rule'). In Passive voice, the subject receives the action (e.g., 'The rule was explained by the teacher'). Passive uses a form of 'to be' + Past Participle (V3).";
            }

            if (lower.Contains("narration") || lower.Contains("direct") || lower.Contains("indirect") || lower.Contains("reported"))
            {
                return "When changing Direct speech into Indirect speech: remember to shift tenses back one step (Present Simple -> Past Simple), change pronouns according to context, and replace reporting verbs like 'said to' with 'told', 'asked', or 'ordered'.";
            }

            if (lower.Contains("preposition") || lower.Contains("in") || lower.Contains("on") || lower.Contains("at"))
            {
                return "Prepositions of time and place: Use 'At' for specific times or points (at 5 PM, at the door), 'On' for days and surfaces (on Monday, on the table), and 'In' for longer periods and enclosed areas (in July, in 2026, in the room).";
            }

            return $"Thank you for your question about: '{userPrompt}'. As your GramaMaster AI tutor, I am here to help you understand English grammar rules, practice error detection, and prepare for contests. Could you provide a specific sentence or rule you would like to analyze together?";
        }
    }
}
