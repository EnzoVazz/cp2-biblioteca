using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Biblioteca.API.HealthChecks;

/// <summary>
/// Check opcional de URL externa (site da FIAP).
/// Falha é reportada como <see cref="HealthStatus.Degraded"/> para não derrubar o status agregado de /health
/// (um check Unhealthy tornaria o relatório inteiro Unhealthy e devolveria HTTP 503).
/// </summary>
public sealed class FiapUrlHealthCheck : IHealthCheck
{
    public const string Name = "fiap";
    private const string Url = "https://www.fiap.com.br";

    private readonly IHttpClientFactory _httpClientFactory;

    public FiapUrlHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("fiap-health");
            using var response = await client.GetAsync(Url, cancellationToken);

            var status = (int)response.StatusCode;
            if (response.IsSuccessStatusCode || status is >= 300 and < 400)
            {
                return HealthCheckResult.Healthy($"FIAP respondeu {status}.");
            }

            return HealthCheckResult.Degraded($"FIAP retornou {status}. A API continua apta a receber tráfego.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Falha ao consultar o site da FIAP. A API continua apta a receber tráfego.", ex);
        }
    }
}
