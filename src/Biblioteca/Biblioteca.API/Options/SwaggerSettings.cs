namespace Biblioteca.API.Options;

/// <summary>
/// Metadados do documento OpenAPI, vinculados à seção <c>Swagger</c> do appsettings.
/// </summary>
public class SwaggerSettings
{
    public const string SectionName = "Swagger";

    public string Title { get; set; } = "Biblioteca API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = string.Empty;
}
