using Domain.Models;
using MediatR;

namespace Core.Application.Customers.List;

public sealed class GetCustomersListQuery : IRequest<List<CustomerWithProductsQueryResult>>;