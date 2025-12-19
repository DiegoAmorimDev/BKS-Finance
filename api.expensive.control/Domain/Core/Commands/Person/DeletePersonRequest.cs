using bks.sdk.Processing.Mediator.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Core.Commands;

public class DeletePersonCommand : IBKSRequest<bool>
{
    public Guid Id { get; init; }
    public string RequestId { get; init; } = string.Empty;
}
