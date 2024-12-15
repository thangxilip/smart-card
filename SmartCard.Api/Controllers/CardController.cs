using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartCard.Application.Domains.Card.Commands;
using SmartCard.Application.Domains.Card.Queries;

namespace SmartCard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController(IMediator sender) : ControllerBase
{
}