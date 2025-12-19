using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Enums;
using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;

namespace Domain.UseCases.QueryHandler;
public class GetTotalsByPersonQueryHandler : IBKSRequestHandler<GetTotalsByPersonQuery, FullReportResponse>
{
    private readonly ITransactionRepository _tRepo;

    public GetTotalsByPersonQueryHandler(ITransactionRepository tRepo) => _tRepo = tRepo;

    public async Task<Result<FullReportResponse>> HandleAsync(GetTotalsByPersonQuery request, CancellationToken ct)
    {
        var data = await _tRepo.GetTotalsByPersonAsync();

        var personList = data.Select(d => {
            IDictionary<string, object> row = d;

            return new TotalsByPersonResponse
            {
                Name = row.ContainsKey("name") ? row["name"]?.ToString() ?? "N/A" : d.Name,
                TotalIncome = (decimal?)(row.ContainsKey("totalincome") ? row["totalincome"] : d.TotalIncome) ?? 0m,
                TotalExpense = (decimal?)(row.ContainsKey("totalexpense") ? row["totalexpense"] : d.TotalExpense) ?? 0m
            };
        }).ToList();

        var report = new FullReportResponse
        {
            PersonTotals = personList,
            GeneralTotalIncome = personList.Sum(x => x.TotalIncome),
            GeneralTotalExpense = personList.Sum(x => x.TotalExpense)
        };

        return Result<FullReportResponse>.Success(report);
    }
}

