using System.Net;

namespace SmartCard.Application.Common.Exceptions;

public class UserFriendlyException(HttpStatusCode code = HttpStatusCode.InternalServerError, string message = "")
    : Exception
{
    public HttpStatusCode Code { get; } = code;
    public override string Message { get; } = message;
}