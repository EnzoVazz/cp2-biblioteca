namespace Biblioteca.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso solicitado não existe.
/// </summary>
public class ResourceNotFoundException : DomainException
{
    public string ResourceName { get; }
    public object? ResourceId { get; }

    public ResourceNotFoundException(string resourceName, object? resourceId)
        : base($"{resourceName} '{resourceId}' não foi encontrado.")
    {
        ResourceName = resourceName;
        ResourceId = resourceId;
    }
}
