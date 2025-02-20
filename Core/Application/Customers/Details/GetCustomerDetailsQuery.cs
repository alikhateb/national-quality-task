using Domain.Models;
using MediatR;

namespace Core.Application.Customers.Details;

public sealed class GetCustomerDetailsQuery(int id) : IRequest<CustomerWithProductsQueryResult>
{
    public int Id => id;
}