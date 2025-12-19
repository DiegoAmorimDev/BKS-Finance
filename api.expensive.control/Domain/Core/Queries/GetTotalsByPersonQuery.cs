using bks.sdk.Processing.Mediator.Abstractions;

namespace Domain.Core.Queries;

public record TotalsByPersonResponse
{
    public string Name { get; init; } = string.Empty;
    public decimal TotalIncome { get; init; }
    public decimal TotalExpense { get; init; }
    public decimal Balance => TotalIncome - TotalExpense;
}

public record FullReportResponse
{
    public IEnumerable<TotalsByPersonResponse> PersonTotals { get; init; } = new List<TotalsByPersonResponse>();
    public decimal GeneralTotalIncome { get; init; }
    public decimal GeneralTotalExpense { get; init; }
    public decimal GeneralBalance => GeneralTotalIncome - GeneralTotalExpense;
}

public class GetTotalsByPersonQuery : IBKSRequest<FullReportResponse>
{
    public string RequestId { get; init; } = Guid.NewGuid().ToString("N");
}