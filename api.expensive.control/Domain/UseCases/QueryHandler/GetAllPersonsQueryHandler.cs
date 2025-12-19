using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;

namespace Domain.UseCases.QueryHandler
{
    public class GetAllPersonsQueryHandler : IBKSRequestHandler<GetAllPersonsQuery, IEnumerable<Person>>
    {
        private readonly IPersonRepository _repo;
        public GetAllPersonsQueryHandler(IPersonRepository repo) => _repo = repo;

        public async Task<Result<IEnumerable<Person>>> HandleAsync(GetAllPersonsQuery request, CancellationToken ct)
        {
            var result = await _repo.GetAllAsync();
            return Result<IEnumerable<Person>>.Success(result);
        }
    }
}
