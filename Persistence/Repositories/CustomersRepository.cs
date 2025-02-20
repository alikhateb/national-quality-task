using Dapper;
using Domain.Models;
using Domain.Repositories;
using System.Data;
using static Domain.Models.CustomerWithProductsQueryResult;

namespace Persistence.Repositories;

public class CustomersRepository(IDbConnection connection) : ICustomersRepository
{
    public async Task AddAsync(CustomerWithProductsCommandModel customer, CancellationToken cancellationToken)
    {
        using (connection)
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    const string query = """
                                         INSERT INTO Customers (Name, Code, DateOfRegistration)
                                         VALUES (@Name, @Code, @DateOfRegistration);
                                         SELECT CAST(SCOPE_IDENTITY() AS int);
                                         """;

                    var customerId = await connection.ExecuteScalarAsync<int>(sql: query,
                        param: customer,
                        transaction: transaction);

                    foreach (var product in customer.Products)
                    {
                        const string insertCustomerProductQuery = """
                                                                  INSERT INTO CustomerProducts (CustomerId, ProductId)
                                                                  VALUES (@CustomerId, @ProductId);
                                                                  """;

                        await connection.ExecuteAsync(sql: insertCustomerProductQuery,
                            param: new
                            {
                                CustomerId = customerId,
                                ProductId = product.Id
                            },
                            transaction: transaction);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<CustomerWithProductsQueryResult> GetByIdWithProductsAsync(int id, CancellationToken cancellationToken)
    {
        const string query = """
                             SELECT
                                 c.Id AS CustomerId,
                                 c.Name AS CustomerName,
                                 c.Code AS CustomerCode,
                                 c.DateOfRegistration,
                                 p.Id AS ProductId,
                                 p.Name AS ProductName,
                                 p.Price
                             FROM Customers c
                             LEFT JOIN CustomerProducts cp
                              ON c.Id = cp.CustomerId
                             LEFT JOIN Products p
                              ON cp.ProductId = p.Id
                             WHERE c.Id = @Id;
                             """;

        using (connection)
        {
            connection.Open();

            var customerDictionary = new Dictionary<int, CustomerWithProductsQueryResult>();

            await connection
                .QueryAsync<CustomerWithProductsQueryResult, ProductResult, CustomerWithProductsQueryResult>(
                    sql: query,
                    map: (customer, product) =>
                    {
                        if (!customerDictionary.TryGetValue(customer.CustomerId, out var customerEntry))
                        {
                            customerEntry = customer;
                            customerDictionary.Add(customer.CustomerId, customerEntry);
                        }

                        customerEntry.Products.Add(product);
                        return customerEntry;
                    },
                    splitOn: "ProductId",
                    param: new
                    {
                        Id = id
                    });

            return customerDictionary.Values.FirstOrDefault()!;
        }
    }

    public async Task<IReadOnlyList<CustomerWithProductsQueryResult>> ListWithProductsAsync(
        CancellationToken cancellationToken)
    {
        const string query = """
                             SELECT
                                 c.Id AS CustomerId,
                                 c.Name AS CustomerName,
                                 c.Code AS CustomerCode,
                                 c.DateOfRegistration,
                                 p.Id AS ProductId,
                                 p.Name AS ProductName,
                                 p.Price
                             FROM Customers c
                             LEFT JOIN CustomerProducts cp
                              ON c.Id = cp.CustomerId
                             LEFT JOIN Products p
                              ON cp.ProductId = p.Id;
                             """;

        using (connection)
        {
            connection.Open();

            var customerDictionary = new Dictionary<int, CustomerWithProductsQueryResult>();

            await connection
                .QueryAsync<CustomerWithProductsQueryResult, ProductResult, CustomerWithProductsQueryResult>(
                    sql: query,
                    map: (customer, product) =>
                    {
                        if (!customerDictionary.TryGetValue(customer.CustomerId, out var customerEntry))
                        {
                            customerEntry = customer;
                            customerDictionary.Add(customer.CustomerId, customerEntry);
                        }

                        customerEntry.Products.Add(product);
                        return customerEntry;
                    },
                    splitOn: "ProductId");

            return customerDictionary.Values.ToList();
        }
    }
}