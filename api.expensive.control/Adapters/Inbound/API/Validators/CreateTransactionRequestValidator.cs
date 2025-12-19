using Adapters.Inbound.API.DTOs.Request;
using FluentValidation;

namespace Adapters.Inbound.API.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("A descrição da transação é obrigatória.");
        RuleFor(x => x.Value).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(x => x.Type).InclusiveBetween(0, 1).WithMessage("Tipo inválido (0: Despesa, 1: Receita).");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("A categoria é obrigatória.");
        RuleFor(x => x.PersonId).NotEmpty().WithMessage("A pessoa é obrigatória.");
    }
}
