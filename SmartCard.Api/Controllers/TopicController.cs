using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCard.Application.Domains.Card.Commands;
using SmartCard.Application.Domains.Card.Queries;
using SmartCard.Application.Domains.Topic.Commands;
using SmartCard.Application.Domains.Topic.Queries;
using SmartCard.Application.Dtos.Card;
using SmartCard.Application.Dtos.Topic;

namespace SmartCard.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TopicController(IMediator sender, IMapper mapper) : ControllerBase
{
    [ProducesResponseType(typeof(List<GetTopicStatisticsOutput>), StatusCodes.Status200OK)]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatisticsAsync()
    {
        var result = await sender.Send(new GetTopicStatisticsQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync(CreateTopicInput input)
    {
        var command = mapper.Map<CreateTopicCommand>(input);
        var result = await sender.Send(command);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetTopicByIdOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var result = await sender.Send(new GetTopicByIdQuery(id));
        return Ok(result);
    }
    
    [HttpGet("{id}/$exercise")]
    [ProducesResponseType(typeof(List<GetDueCardsOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCardsForStudyingAsync(Guid id)
    {
        var result = await sender.Send(new GetDueCardsQuery(id));
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, UpdateTopicInput input)
    {
        input.Id = id;
        var command = new UpdateTopicCommand(input);
        await sender.Send(command);
        return Ok(id);
    }
    
    [HttpPatch("{id}/$score")]
    public async Task<IActionResult> ScoreAsync([FromRoute] Guid id, List<ScoreInput> cards)
    {
        var command = new ScoreCardsCommand(id, cards);
        await sender.Send(command);
        return Ok();
    }
}