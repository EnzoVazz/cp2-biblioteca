using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.API.Controllers;

/// <summary>
/// Endpoints do acervo de livros.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LivrosController : ControllerBase
{
    private readonly IRepository<Livro> _livroRepository;
    private readonly IRepository<BibliotecaEntity> _bibliotecaRepository;

    public LivrosController(
        IRepository<Livro> livroRepository,
        IRepository<BibliotecaEntity> bibliotecaRepository)
    {
        _livroRepository = livroRepository;
        _bibliotecaRepository = bibliotecaRepository;
    }

    /// <summary>
    /// Lista todos os livros do acervo.
    /// </summary>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LivroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<LivroResponse>>> GetAll()
    {
        var livros = await _livroRepository.GetAllAsync();
        return Ok(livros.Select(LivroResponse.From));
    }

    /// <summary>
    /// Busca um livro pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do livro.</param>
    /// <response code="200">Livro encontrado.</response>
    /// <response code="400">Identificador inválido.</response>
    /// <response code="404">Livro não encontrado.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LivroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LivroResponse>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O identificador informado é inválido.");

        var livro = await _livroRepository.GetByIdAsync(id)
                    ?? throw new ResourceNotFoundException("Livro", id);

        return Ok(LivroResponse.From(livro));
    }

    /// <summary>
    /// Cadastra um novo livro vinculado a uma biblioteca existente.
    /// </summary>
    /// <param name="request">Dados do volume e identificador da biblioteca.</param>
    /// <response code="201">Livro criado.</response>
    /// <response code="400">Dados inválidos (título, descrição, páginas ou data).</response>
    /// <response code="404">Biblioteca informada não existe.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(LivroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LivroResponse>> Create([FromBody] CreateLivroRequest request)
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

        var response = LivroResponse.From(livro);
        return CreatedAtAction(nameof(GetById), new { id = response.IdLivro }, response);
    }
}
