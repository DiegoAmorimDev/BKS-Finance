using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using Domain.Core.Ports.Outbound;

public class CreateCategoryCommandHandler : IBKSRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    // Injetando o logger e o repositório.

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<Result<CreateCategoryResponse>> HandleAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Description, (TransactionPurpose)request.Purpose);
        await _categoryRepository.AddAsync(category);

        return Result<CreateCategoryResponse>.Success(new CreateCategoryResponse
        {
            Id = category.Id,
            Message = "Categoria criada com sucesso!"
        });
    }
}
