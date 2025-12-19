using bks.sdk.Processing.Mediator.Abstractions;

public record CreateCategoryResponse
{
    public Guid Id { get; init; }
    public string Message { get; init; } = string.Empty;
}

public class CreateCategoryCommand : IBKSRequest<CreateCategoryResponse>
{
    public string Description { get; init; } = string.Empty;
    public int Purpose { get; init; } // Mapeado para o Enum TransactionPurpose (0: Expense, 1: Income, 2: Both)
    public string RequestId { get; init; } = string.Empty;
}
