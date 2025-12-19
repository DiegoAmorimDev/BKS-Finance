using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;

namespace Domain.Core.Queries
{
    public class GetAllCategoriesQuery : IBKSRequest<IEnumerable<Category>> { public string RequestId { get; set; } = string.Empty; }
}
