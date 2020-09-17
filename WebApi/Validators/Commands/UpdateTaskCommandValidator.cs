using Domain.Commands;
using FluentValidation;
using System;

namespace WebApi.Validators.Commands
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Task id is required.")
                .Must((data, id) => Guid.TryParse(data.Id.ToString(), out var _)).WithMessage("Valid task id is required.");

            RuleFor(c => c.Subject)
               .Must((data, name) => !string.IsNullOrEmpty(data.Subject)).WithMessage("Subject is required.")
               .MaximumLength(255).WithMessage("Subject cannot exceed 255 characters.");
        }
    }
}
