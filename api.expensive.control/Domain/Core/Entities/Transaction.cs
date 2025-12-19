using Domain.Core.Enums;

namespace Domain.Core.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Value { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid PersonId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Construtor principal para o código de negócio
    public Transaction(string description, decimal value, TransactionType type, Guid categoryId, Guid personId)
    {
        Id = Guid.NewGuid();
        Description = description;
        Value = value;
        Type = type;
        CategoryId = categoryId;
        PersonId = personId;
        CreatedAt = DateTime.UtcNow;
    }

    // SOLUÇÃO DO ERRO: Construtor sem parâmetros exigido pelo Dapper para materialização
    // Ele pode ser privado, o Dapper consegue usá-lo via Reflection.
    private Transaction() { }

    // Método para o Dapper preencher os campos se as colunas forem snake_case no banco
    // Ou você pode adicionar um construtor que combine com a assinatura do erro
    public Transaction(Guid id, string description, decimal value, int type, Guid category_id, Guid person_id, DateTime created_at)
    {
        Id = id;
        Description = description;
        Value = value;
        Type = (TransactionType)type;
        CategoryId = category_id;
        PersonId = person_id;
        CreatedAt = created_at;
    }
}