using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands.Transaction;
using Domain.Core.Enums;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;

public class CreateTransactionCommandHandler : IBKSRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
{
    private readonly ITransactionRepository _tRepo;
    private readonly IPersonRepository _pRepo;
    private readonly ICategoryRepository _cRepo;

    public CreateTransactionCommandHandler(ITransactionRepository tRepo, IPersonRepository pRepo, ICategoryRepository cRepo)
    {
        _tRepo = tRepo;
        _pRepo = pRepo;
        _cRepo = cRepo;
    }

    public async Task<Result<CreateTransactionResponse>> HandleAsync(CreateTransactionCommand request, CancellationToken ct)
    {
        // 1. Validar Pessoa
        var person = await _pRepo.GetByIdAsync(request.PersonId);
        if (person == null) return Result<CreateTransactionResponse>.Failure("Pessoa não encontrada.");

        // REGRA: Menor de idade (menor de 18) apenas despesas (Type 0: Despesa, 1: Receita)
        if (person.Age < 18 && request.Type == 1)
        {
            return Result<CreateTransactionResponse>.Failure("Menores de 18 anos só podem registrar transações do tipo Despesa.");
        }

        // 2. Validar Categoria
        var category = await _cRepo.GetByIdAsync(request.CategoryId);
        if (category == null) return Result<CreateTransactionResponse>.Failure("Categoria não encontrada.");

        // REGRA: Restringir categorias conforme finalidade (0: Despesa, 1: Receita, 2: Ambas)
        if (category.Purpose != TransactionPurpose.Both)
        {
            if ((int)category.Purpose != request.Type)
            {
                var finalidadeDesc = category.Purpose == TransactionPurpose.Expense ? "Despesa" : "Receita";
                return Result<CreateTransactionResponse>.Failure($"A categoria '{category.Description}' só permite transações do tipo {finalidadeDesc}.");
            }
        }

        // 3. Persistência
        var transaction = new Transaction(request.Description, request.Value, (TransactionType)request.Type, request.CategoryId, request.PersonId);
        await _tRepo.AddAsync(transaction);

        return Result<CreateTransactionResponse>.Success(new CreateTransactionResponse
        {
            Id = transaction.Id,
            Message = "Transação registrada com sucesso!"
        });
    }
}