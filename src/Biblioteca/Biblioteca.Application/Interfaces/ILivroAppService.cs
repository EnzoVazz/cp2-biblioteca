using Biblioteca.Application.DTOs;

namespace Biblioteca.Application.Interfaces;

/// <summary>
/// Caso de uso de cadastro de livros no acervo.
/// </summary>
public interface ILivroAppService
{
    /// <summary>
    /// Cria um livro vinculado a uma biblioteca existente.
    /// </summary>
    /// <exception cref="Domain.Exceptions.ResourceNotFoundException">Biblioteca informada não existe.</exception>
    /// <exception cref="Domain.Exceptions.DomainException">Dados do livro violam regra de negócio.</exception>
    Task<LivroResponse> CreateAsync(CreateLivroRequest request);
}
