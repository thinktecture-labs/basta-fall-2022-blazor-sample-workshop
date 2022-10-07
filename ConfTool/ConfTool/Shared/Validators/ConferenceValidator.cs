using ConfTool.Shared.DTO;
using FluentValidation;

namespace ConfTool.Shared.Validators
{
    public class ConferenceValidator : AbstractValidator<ConferenceDetail>
    {
        public ConferenceValidator()
        {
            RuleFor(c => c.DateTo).GreaterThanOrEqualTo(c => c.DateFrom)
                .WithMessage("Das Enddatum muss vor dem Anfangsdatum liegen.");
        }
    }
}
