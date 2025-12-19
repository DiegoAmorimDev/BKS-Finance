using Domain.Core.Commands;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Enums;
using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.UseCases;

/* -------------------------------------------------------------------------
   HANDLER: CRIAR PESSOA
   ------------------------------------------------------------------------- */
public class CreatePersonCommandHandler : IBKSRequestHandler<CreatePersonCommand, CreatePersonResponse>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<CreatePersonCommandHandler> _logger;

    public CreatePersonCommandHandler(IPersonRepository personRepository, ILogger<CreatePersonCommandHandler> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }

    public async Task<Result<CreatePersonResponse>> HandleAsync(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Criando pessoa {Name}", request.Name);

        var person = new Person(request.Name, request.Age);
        await _personRepository.AddAsync(person);

        var response = new CreatePersonResponse
        {
            Id = person.Id,
            Message = "Pessoa cadastrada com sucesso!",
            CreatedAt = DateTime.UtcNow
        };

        return Result<CreatePersonResponse>.Success(response);
    }
}