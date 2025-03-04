using AutoMapper;
using SmartCard.Application.Domains.Topic.Commands;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;
using SmartCard.Domain.Entities;

namespace SmartCard.Api.Mappers;

public class ApiMapperProfile : Profile
{
    public ApiMapperProfile()
    {
        // topic
        CreateMap<CreateTopicInput, CreateTopicCommand>();
        
        // card
        CreateMap<Card, GetDueCardsOutput>();
    }
}