using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;

namespace Domain.UseCases.QueryHandler
{
    public class GetAllTransactionsQueryHandler : IBKSRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionListItemResponse>>
    {
        private readonly ITransactionRepository _tRepo;
        private readonly IPersonRepository _pRepo;
        private readonly ICategoryRepository _cRepo;

        public GetAllTransactionsQueryHandler(ITransactionRepository tRepo, IPersonRepository pRepo, ICategoryRepository cRepo)
        {
            _tRepo = tRepo;
            _pRepo = pRepo;
            _cRepo = cRepo;
        }

        public async Task<Result<IEnumerable<TransactionListItemResponse>>> HandleAsync(GetAllTransactionsQuery request, CancellationToken ct)
        {
            var transactions = (await _tRepo.GetAllAsync()) ?? Enumerable.Empty<Transaction>();
            var persons = (await _pRepo.GetAllAsync()) ?? Enumerable.Empty<Person>();
            var categories = (await _cRepo.GetAllAsync()) ?? Enumerable.Empty<Category>();

            var response = transactions.Select(t => new TransactionListItemResponse
            {
                Id = t.Id,
                Description = t.Description,
                Value = t.Value,
                Type = (int)t.Type,
                CategoryId = t.CategoryId,
                CategoryDescription = categories.FirstOrDefault(c => c.Id == t.CategoryId)?.Description ?? "N/A",
                PersonId = t.PersonId,
                PersonName = persons.FirstOrDefault(p => p.Id == t.PersonId)?.Name ?? "N/A",
                CreatedAt = t.CreatedAt
            }).ToList();

            return Result<IEnumerable<TransactionListItemResponse>>.Success(response);
        }
    }
}
