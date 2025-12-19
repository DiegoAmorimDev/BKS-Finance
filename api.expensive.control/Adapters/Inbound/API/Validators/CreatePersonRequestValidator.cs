using Adapters.Inbound.API.DTOs.Request;
using FluentValidation;

namespace Adapters.Inbound.API.Validators;

    public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
    {
        public CreatePersonRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            // Invalida idade 0
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("A idade deve ser maior que zero.");
        }
    }
