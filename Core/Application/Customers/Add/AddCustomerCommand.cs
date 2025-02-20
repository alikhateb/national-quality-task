using FluentValidation;
using MediatR;

namespace Core.Application.Customers.Add;

public sealed class AddCustomerCommand : IRequest
{
    public string Name { get; set; }

    public string Code { get; set; }

    public DateOnly DateOfRegistration { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public List<CustomerProductsModel> Products { get; set; } = [];
}

public sealed class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    public AddCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .NotNull()
            .WithMessage("Name is required");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .NotNull()
            .WithMessage("Code is required");

        RuleFor(x => x.DateOfRegistration)
            .Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("DateOfRegistration should not be in the past");

        RuleFor(x => x.Products)
            .Must(x => x.Count > 0)
            .WithMessage("product list is empty");

        RuleForEach(x => x.Products)
            .SetValidator(new CustomerProductsModelValidator());
    }
}