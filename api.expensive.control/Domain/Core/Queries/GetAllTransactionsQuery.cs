using bks.sdk.Processing.Mediator.Abstractions;

namespace Domain.Core.Queries
{
    public record TransactionListItemResponse
    {
        public Guid Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public decimal Value { get; init; }
        public int Type { get; init; }
        public Guid CategoryId { get; init; }
        public string CategoryDescription { get; init; } = string.Empty;
        public Guid PersonId { get; init; }
        public string PersonName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    public class GetAllTransactionsQuery : IBKSRequest<IEnumerable<TransactionListItemResponse>>
    {
        public string RequestId { get; init; } = Guid.NewGuid().ToString("N");
    }


}
