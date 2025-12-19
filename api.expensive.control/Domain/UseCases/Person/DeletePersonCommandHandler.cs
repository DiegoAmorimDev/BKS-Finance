using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Ports.Outbound;

public class DeletePersonCommandHandler : IBKSRequestHandler<DeletePersonCommand, bool>
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<DeletePersonCommandHandler> _logger;

    public DeletePersonCommandHandler(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository,
        ILogger<DeletePersonCommandHandler> logger)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Removendo pessoa e transações para ID {Id}", request.Id);

        var person = await _personRepository.GetByIdAsync(request.Id);
        if (person == null) return Result<bool>.Failure("Pessoa não encontrada.");

        // Implementação do Cascade manual exigido
        await _transactionRepository.DeleteByPersonIdAsync(request.Id);
        await _personRepository.DeleteAsync(request.Id);

        return Result<bool>.Success(true);
    }
}