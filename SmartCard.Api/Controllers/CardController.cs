using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCard.Application.Domains.Card.Commands;
using SmartCard.Application.Domains.Card.Queries;
using SmartCard.Application.Dtos.Card;

namespace SmartCard.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CardController(IMediator sender, IMapper mapper) : ControllerBase
{
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [HttpPost("revise")]
    public async Task<IActionResult> ReviseCard(ReviseCardCommand command)
    {
        var result = await sender.Send(command);
        return Ok(result);
    }
    
    [ProducesResponseType(typeof(List<GetDueCardsOutput>), StatusCodes.Status200OK)]
    [HttpGet("due")]
    public async Task<IActionResult> GetDueCardsAsync(Guid topicId)
    {
        var result = await sender.Send(new GetDueCardsQuery(topicId));
        return Ok(mapper.Map<List<GetDueCardsOutput>>(result));
    }
}