namespace Biblioteca.Application.DTOs;

/// <summary>
/// Payload para cadastro de um livro.
/// </summary>
public class CreateLivroRequest
{
    /// <summary>Título do livro.</summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>Série (pode ser vazia para volume único).</summary>
    public string Serie { get; set; } = string.Empty;

    /// <summary>Sinopse ou descrição do volume.</summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Data de lançamento (não pode ser futura).</summary>
    public DateOnly DataLancamento { get; set; }

    /// <summary>Quantidade de páginas (maior que zero).</summary>
    public int NPaginas { get; set; }

    /// <summary>Identificador da biblioteca à qual o livro pertence.</summary>
    public Guid IdBiblioteca { get; set; }
}

/// <summary>
/// Representação de um livro na API (sem navegações de domínio).
/// </summary>
public class LivroResponse
{
    public Guid IdLivro { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateOnly DataLancamento { get; set; }
    public int NPaginas { get; set; }
    public Guid IdBiblioteca { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public static LivroResponse From(Domain.Entities.Livro livro) => new()
    {
        IdLivro = livro.IdLivro,
        Titulo = livro.Titulo,
        Serie = livro.Serie,
        Descricao = livro.Descricao,
        DataLancamento = livro.DataLancamento,
        NPaginas = livro.NPaginas,
        IdBiblioteca = livro.IdBiblioteca,
        Active = livro.Active,
        CreatedAt = livro.CreatedAt
    };
}
