using System.Reflection;
using Biblioteca.API.Options;
using Microsoft.OpenApi;

namespace Biblioteca.API.Extensions;

/// <summary>
/// Configuração centralizada do Swagger/OpenAPI.
/// </summary>
public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddBibliotecaSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()
                       ?? new SwaggerSettings();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = "Grupo CP3 — Sistema de Biblioteca"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }

            var applicationXml = Path.Combine(AppContext.BaseDirectory, "Biblioteca.Application.xml");
            if (File.Exists(applicationXml))
            {
                options.IncludeXmlComments(applicationXml);
            }
        });

        return services;
    }
}
