using bks.sdk.Processing.Mediator.Abstractions;

namespace Domain.Core.Commands.Transaction;

public record CreateTransactionResponse
{
    public Guid Id { get; init; }
    public string Message { get; init; } = string.Empty;
}

public class CreateTransactionCommand : IBKSRequest<CreateTransactionResponse>
{
    public string Description { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public int Type { get; init; } // Mapeado para TransactionType (0: Expense, 1: Income)
    public Guid CategoryId { get; init; }
    public Guid PersonId { get; init; }
    public string RequestId { get; init; } = string.Empty;
}
