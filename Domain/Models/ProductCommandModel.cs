namespace Domain.Models;

public class ProductCommandModel
{
    public required string Name { get; init; }

    public required decimal Price { get; init; }
}