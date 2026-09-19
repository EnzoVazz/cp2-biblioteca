using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.Application.Services;

/// <summary>
/// Orquestra o cadastro de livros: valida o domínio e a existência da biblioteca antes de persistir.
/// </summary>
public sealed class LivroAppService : ILivroAppService
{
    private readonly IRepository<Livro> _livroRepository;
    private readonly IRepository<BibliotecaEntity> _bibliotecaRepository;

    public LivroAppService(
        IRepository<Livro> livroRepository,
        IRepository<BibliotecaEntity> bibliotecaRepository)
    {
        _livroRepository = livroRepository;
        _bibliotecaRepository = bibliotecaRepository;
    }

    public async Task<LivroResponse> CreateAsync(CreateLivroRequest request)
    {
        var livro = new Livro(
            request.Titulo,
            request.Serie,
            request.Descricao,
            request.DataLancamento,
            request.NPaginas,
            request.IdBiblioteca);

        if (!await _bibliotecaRepository.ExistsByIdAsync(request.IdBiblioteca))
            throw new ResourceNotFoundException("Biblioteca", request.IdBiblioteca);

        await _livroRepository.AddAsync(livro);
        return LivroResponse.From(livro);
    }
}
