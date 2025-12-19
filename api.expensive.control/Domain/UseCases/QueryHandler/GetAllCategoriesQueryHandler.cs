using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;
using Domain.Core.Queries;
using Domain.Core.Ports.Outbound;


namespace Domain.UseCases.QueryHandler
{
    public class GetAllCategoriesQueryHandler : IBKSRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _repo;
        public GetAllCategoriesQueryHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<Result<IEnumerable<Category>>> HandleAsync(GetAllCategoriesQuery request, CancellationToken ct)
        {
            var result = await _repo.GetAllAsync();
            return Result<IEnumerable<Category>>.Success(result);
        }
    }
}
