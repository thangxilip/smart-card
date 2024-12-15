using AutoMapper;
using SmartCard.Application.Domains.Topic.Commands;
using SmartCard.Application.Dtos.Topic;

namespace SmartCard.Api.Mappers;

public class ApiMapperProfile : Profile
{
    public ApiMapperProfile()
    {
        CreateMap<CreateTopicInput, CreateTopicCommand>();
    }
}