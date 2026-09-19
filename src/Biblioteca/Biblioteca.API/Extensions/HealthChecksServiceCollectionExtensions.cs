using Biblioteca.API.HealthChecks;
using Biblioteca.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Biblioteca.API.Extensions;

/// <summary>
/// Registro dos health checks da API (processo, Oracle via DbContext e URL da FIAP).
/// </summary>
public static class HealthChecksServiceCollectionExtensions
{
    public static IServiceCollection AddBibliotecaHealthChecks(this IServiceCollection services)
    {
        services.AddHttpClient("fiap-health", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BibliotecaAPI-HealthCheck/1.0");
        });

        services.AddHealthChecks()
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy("Processo da API em execução."),
                tags: ["self"])
            .AddDbContextCheck<BibliotecaContext>(
                name: "oracle",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "oracle"])
            .AddCheck<FiapUrlHealthCheck>(
                FiapUrlHealthCheck.Name,
                tags: ["external"]);

        return services;
    }
}
