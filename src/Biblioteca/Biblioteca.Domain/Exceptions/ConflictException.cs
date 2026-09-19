namespace Biblioteca.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando a operação conflita com o estado atual do recurso (HTTP 409).
/// </summary>
public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message)
    {
    }
}
