using bks.sdk.Processing.Mediator.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Core.Commands;

public record CreatePersonResponse
{
    public Guid Id { get; init; }
    public string Message { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public class CreatePersonCommand : IBKSRequest<CreatePersonResponse>
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string RequestId { get; init; } = string.Empty;
}