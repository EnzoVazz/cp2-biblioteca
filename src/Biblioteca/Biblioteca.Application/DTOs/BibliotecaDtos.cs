namespace Biblioteca.Application.DTOs;

/// <summary>
/// Payload para cadastro de uma biblioteca.
/// </summary>
public class CreateBibliotecaRequest
{
    /// <summary>Nome da unidade.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Endereço com no mínimo 5 caracteres.</summary>
    public string Endereco { get; set; } = string.Empty;
}

/// <summary>
/// Representação de uma biblioteca na API.
/// </summary>
public class BibliotecaResponse
{
    public Guid IdBiblioteca { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public static BibliotecaResponse From(Domain.Entities.Biblioteca biblioteca) => new()
    {
        IdBiblioteca = biblioteca.IdBiblioteca,
        Nome = biblioteca.Nome,
        Endereco = biblioteca.Endereco,
        Active = biblioteca.Active,
        CreatedAt = biblioteca.CreatedAt
    };
}
