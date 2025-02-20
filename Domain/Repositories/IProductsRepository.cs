using Domain.Models;

namespace Domain.Repositories;

public interface IProductsRepository
{
    Task AddAsync(ProductCommandModel productCommandModel, CancellationToken cancellationToken);

    Task<IReadOnlyList<ProductQueryResult>> ListAsync(CancellationToken cancellationToken);
}