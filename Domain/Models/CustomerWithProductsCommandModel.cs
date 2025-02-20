namespace Domain.Models;

public class CustomerWithProductsCommandModel
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateOnly DateOfRegistration { get; set; }

    public List<ProductCommandModel> Products { get; set; } = [];

    public class ProductCommandModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}