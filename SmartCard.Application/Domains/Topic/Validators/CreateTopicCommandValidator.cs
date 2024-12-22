using FluentValidation;
using SmartCard.Application.Domains.Topic.Commands;

namespace SmartCard.Application.Domains.Topic.Validators;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");
        
        RuleFor(x => x.Cards)
            .NotEmpty()
            .Must(x => x.Count > 0)
            .WithMessage("At least one card must be added");
    }
}