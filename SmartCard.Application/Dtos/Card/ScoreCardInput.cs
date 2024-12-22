using SmartCard.Domain.Enums;

namespace SmartCard.Application.Dtos.Card;

public class ScoreInput
{
    public Guid Id { get; set; }

    public Score Score { get; set; }
}