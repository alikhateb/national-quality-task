using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Core.Application.Customers.Add;

public sealed class AddCustomerCommandHandler(ICustomersRepository customersRepository)
    : IRequestHandler<AddCustomerCommand>
{
    public async Task Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var command = new CustomerWithProductsCommandModel
        {
            Name = request.Name,
            Code = request.Code,
            DateOfRegistration = request.DateOfRegistration,
            Products = request.Products.Select(x => new CustomerWithProductsCommandModel.ProductCommandModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).ToList()
        };

        await customersRepository.AddAsync(command, cancellationToken);
    }
}