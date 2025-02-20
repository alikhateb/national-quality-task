using Domain.Models;
using MediatR;

namespace Core.Application.Products.List;

public sealed class GetProductsListQuery : IRequest<List<ProductQueryResult>>;