namespace Biblioteca.Application.DTOs;

/// <summary>
/// Payload para cadastro de um gênero literário.
/// </summary>
public class CreateGeneroRequest
{
    /// <summary>Nome do gênero (obrigatório).</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Descrição com no mínimo 10 caracteres.</summary>
    public string Descricao { get; set; } = string.Empty;
}

/// <summary>
/// Payload para atualização de um gênero literário.
/// </summary>
public class UpdateGeneroRequest
{
    /// <summary>Novo nome do gênero.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Nova descrição (mínimo de 10 caracteres).</summary>
    public string Descricao { get; set; } = string.Empty;
}

/// <summary>
/// Representação de um gênero na API.
/// </summary>
public class GeneroResponse
{
    public Guid IdGenero { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public static GeneroResponse From(Domain.Entities.Genero genero) => new()
    {
        IdGenero = genero.IdGenero,
        Nome = genero.Nome,
        Descricao = genero.Descricao,
        Active = genero.Active,
        CreatedAt = genero.CreatedAt
    };
}
