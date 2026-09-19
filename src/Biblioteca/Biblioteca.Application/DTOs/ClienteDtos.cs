namespace Biblioteca.Application.DTOs;

/// <summary>
/// Payload para cadastro de um cliente. A senha nunca é devolvida nas respostas.
/// </summary>
public class CreateClienteRequest
{
    /// <summary>Nome completo.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>E-mail de contato (deve conter @).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Data de nascimento (cliente deve ser maior de 18 anos).</summary>
    public DateOnly DataNascimento { get; set; }

    /// <summary>Senha em texto plano; será hasheada no domínio.</summary>
    public string Senha { get; set; } = string.Empty;

    /// <summary>Biblioteca à qual o cliente está vinculado.</summary>
    public Guid IdBiblioteca { get; set; }
}

/// <summary>
/// Representação de um cliente na API (sem senha nem salt).
/// </summary>
public class ClienteResponse
{
    public Guid IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public int Idade { get; set; }
    public Guid IdBiblioteca { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public static ClienteResponse From(Domain.Entities.Cliente cliente) => new()
    {
        IdCliente = cliente.IdCliente,
        Nome = cliente.Nome,
        Email = cliente.Email,
        DataNascimento = cliente.DataNascimento,
        Idade = cliente.Idade,
        IdBiblioteca = cliente.IdBiblioteca,
        Active = cliente.Active,
        CreatedAt = cliente.CreatedAt
    };
}
