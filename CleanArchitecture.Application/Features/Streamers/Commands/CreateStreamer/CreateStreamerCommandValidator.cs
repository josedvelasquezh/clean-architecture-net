using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    public class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommand>
    {
        public CreateStreamerCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{Name} no puede estar vacio")
                .NotNull().WithMessage("{Name} no puede ser null")
                .MaximumLength(50).WithMessage("{Name} maximo 50");

            RuleFor(p => p.Url)
                .NotEmpty().WithMessage("{Url} no puede estar vacio");
        }
    }
}
