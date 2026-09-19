using Biblioteca.Application.Interfaces;
using Biblioteca.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Application;

/// <summary>
/// Composição da camada Application (serviços de caso de uso).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILivroAppService, LivroAppService>();
        return services;
    }
}
