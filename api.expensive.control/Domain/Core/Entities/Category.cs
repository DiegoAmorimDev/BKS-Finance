using Domain.Core.Enums;

namespace Domain.Core.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public TransactionPurpose Purpose { get; private set; }

    public Category(string description, TransactionPurpose purpose)
    {
        Validate(description);

        Id = Guid.NewGuid();
        Description = description;
        Purpose = purpose;
    }

    // Construtor para reconstrução via Dapper/Infra
    public Category(Guid id, string description, TransactionPurpose purpose)
    {
        Id = id;
        Description = description;
        Purpose = purpose;
    }

    private void Validate(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da categoria é obrigatória.");
    }
}