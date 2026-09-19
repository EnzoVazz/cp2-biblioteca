using Biblioteca.Application.Interfaces;
using Biblioteca.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Infrastructure;

/// <summary>
/// Composição da camada de Infrastructure: DbContext Oracle e repositório genérico.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OracleConnection")
            ?? throw new InvalidOperationException("Connection string 'OracleConnection' não configurada.");

        services.AddDbContext<BibliotecaContext>(options =>
            options.UseOracle(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
