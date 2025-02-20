using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Core.Application.Customers.Details;

public sealed class GetCustomerDetailsQueryHandler(ICustomersRepository customersRepository)
    : IRequestHandler<GetCustomerDetailsQuery, CustomerWithProductsQueryResult>
{
    public async Task<CustomerWithProductsQueryResult> Handle(GetCustomerDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await customersRepository.GetByIdWithProductsAsync(request.Id, cancellationToken);
        return customer;
    }
}