using System.Data;
using Dapper;
using Domain.Models;
using Domain.Repositories;

namespace Persistence.Repositories;

public class ProductsRepository(IDbConnection connection) : IProductsRepository
{
    public async Task AddAsync(ProductCommandModel productCommandModel, CancellationToken cancellationToken)
    {
        using (connection)
        {
            connection.Open();

            const string query = """
                                 INSERT INTO Products (Name, Price)
                                 VALUES (@Name, @Price);
                                 SELECT CAST(SCOPE_IDENTITY() AS int);
                                 """;

            await connection.ExecuteScalarAsync<int>(sql: query, param: productCommandModel);
        }
    }

    public async Task<IReadOnlyList<ProductQueryResult>> ListAsync(CancellationToken cancellationToken)
    {
        const string query = "SELECT * FROM Products;";

        using (connection)
        {
            connection.Open();

            var products = await connection.QueryAsync<ProductQueryResult>(sql: query);

            return products.ToList();
        }
    }
}