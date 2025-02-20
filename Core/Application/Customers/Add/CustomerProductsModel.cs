using FluentValidation;

namespace Core.Application.Customers.Add;

public sealed class CustomerProductsModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

public sealed class CustomerProductsModelValidator : AbstractValidator<CustomerProductsModel>
{
    public CustomerProductsModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("invalid product Name");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("invalid product Price");
    }
}