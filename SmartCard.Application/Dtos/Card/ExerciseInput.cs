using SmartCard.Domain.Enums;

namespace SmartCard.Application.Dtos.Card;

public class ExerciseInput
{
    public Guid CardId { get; set; }

    public Score Score { get; set; }
}