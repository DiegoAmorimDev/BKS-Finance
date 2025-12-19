using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;

namespace Domain.Core.Queries
{
    public class GetAllPersonsQuery : IBKSRequest<IEnumerable<Person>> { public string RequestId { get; set; } = string.Empty; }
}
