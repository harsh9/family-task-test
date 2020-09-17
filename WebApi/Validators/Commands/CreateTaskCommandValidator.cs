using Domain.Commands;
using FluentValidation;

namespace WebApi.Validators.Commands
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {          
            RuleFor(c => c.Subject)
                .Must((data, name) => !string.IsNullOrEmpty(data.Subject)).WithMessage("Subject is required.")
                .MaximumLength(255).WithMessage("Subject cannot exceed 255 characters.");
        }
    }
}
