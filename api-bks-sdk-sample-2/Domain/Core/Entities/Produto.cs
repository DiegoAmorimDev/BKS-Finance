namespace Domain.Core.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
