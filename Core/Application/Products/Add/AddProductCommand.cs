using FluentValidation;
using MediatR;

namespace Core.Application.Products.Add;

public sealed class AddProductCommand : IRequest
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
}

public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .NotNull()
            .WithMessage("Name is required");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("price should be greater than 0");
    }
}