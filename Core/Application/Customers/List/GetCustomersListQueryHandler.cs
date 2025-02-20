using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Core.Application.Customers.List;

public sealed class GetCustomersListQueryHandler(ICustomersRepository customersRepository)
    : IRequestHandler<GetCustomersListQuery, List<CustomerWithProductsQueryResult>>
{
    public async Task<List<CustomerWithProductsQueryResult>> Handle(GetCustomersListQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await customersRepository.ListWithProductsAsync(cancellationToken);
        return customers.ToList();
    }
}