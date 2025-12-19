namespace Adapters.Inbound.API.DTOs.Request;

public record CreateTransactionRequest(
    string Description,
    decimal Value,
    int Type,
    Guid CategoryId,
    Guid PersonId
);