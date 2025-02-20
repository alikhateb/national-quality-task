using Domain.Models;

namespace Domain.Repositories;

public interface ICustomersRepository
{
    Task AddAsync(CustomerWithProductsCommandModel customer, CancellationToken cancellationToken);

    Task<CustomerWithProductsQueryResult> GetByIdWithProductsAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<CustomerWithProductsQueryResult>> ListWithProductsAsync(CancellationToken cancellationToken);
}