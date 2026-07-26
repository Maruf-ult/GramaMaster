using AutoMapper;
using GramaMaster.Application.DTOs.Topics;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class TopicProfile:Profile
    {
        public TopicProfile()
        {
            CreateMap<CreateTopicDto, Topic>();
            CreateMap<UpdateTopicDto, Topic>();
            CreateMap<Topic, TopicDto>();
            CreateMap<Topic, TopicAnalyticsDto>();
        }
    }
}
