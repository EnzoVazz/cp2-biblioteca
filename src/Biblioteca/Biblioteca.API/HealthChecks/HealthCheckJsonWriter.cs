using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Biblioteca.API.HealthChecks;

/// <summary>
/// Serializa o relatório de health check em JSON (status, duração e lista de checks).
/// Detalhe de exceção só é incluído em Development.
/// </summary>
public static class HealthCheckJsonWriter
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
        var includeException = environment.IsDevelopment();

        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = Math.Round(report.TotalDuration.TotalMilliseconds, 2),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                durationMs = Math.Round(entry.Value.Duration.TotalMilliseconds, 2),
                description = entry.Value.Description,
                exception = includeException ? entry.Value.Exception?.Message : null
            })
        };

        await context.Response.WriteAsJsonAsync(payload, SerializerOptions, context.RequestAborted);
    }
}
