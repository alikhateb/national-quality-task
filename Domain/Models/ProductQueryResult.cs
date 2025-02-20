namespace Domain.Models;

public class ProductQueryResult
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
}