namespace Domain.Models;

public sealed class CustomerWithProductsQueryResult
{
    public int CustomerId { get; init; }

    public string CustomerName { get; init; }

    public string CustomerCode { get; init; }

    public DateOnly DateOfRegistration { get; init; }

    public List<ProductResult> Products { get; set; } = [];

    public sealed class ProductResult
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal Price { get; init; }
    }
}