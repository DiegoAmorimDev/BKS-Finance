using Adapters.Inbound.API.DTOs.Request;
using FluentValidation;

namespace Adapters.Inbound.API.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("A descrição é obrigatória.");
        RuleFor(x => x.Purpose).InclusiveBetween(0, 2).WithMessage("Finalidade inválida (0: Despesa, 1: Receita, 2: Ambas).");
    }
}