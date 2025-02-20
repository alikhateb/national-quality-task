using Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Persistence.Repositories;
using Dapper;

namespace Persistence;

public static class DependencyInjection
{
    public static void RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyHandler());

        services.AddScoped<ICustomersRepository, CustomersRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
    }
}