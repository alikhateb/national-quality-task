using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Core.Application.Products.List;

public sealed class GetProductsListQueryHandler(IProductsRepository productsRepository)
    : IRequestHandler<GetProductsListQuery, List<ProductQueryResult>>
{
    public async Task<List<ProductQueryResult>> Handle(GetProductsListQuery request,
        CancellationToken cancellationToken)
    {
        var products = await productsRepository.ListAsync(cancellationToken);
        return products.ToList();
    }
}