using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Core.Application.Products.Add;

public sealed class AddProductCommandHandler(IProductsRepository productsRepository)
    : IRequestHandler<AddProductCommand>
{
    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new ProductCommandModel
        {
            Name = request.Name,
            Price = request.Price,
        };

        await productsRepository.AddAsync(product, cancellationToken);
    }
}