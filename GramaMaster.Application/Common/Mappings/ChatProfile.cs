using AutoMapper;
using GramaMaster.Application.DTOs.Chats;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>();

            CreateMap<ChatSession, ChatHistoryDto>();

            CreateMap<ChatMessage, ChatResponseDto>();
        }
    }
}
