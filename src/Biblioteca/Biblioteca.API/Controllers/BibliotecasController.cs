using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.API.Controllers;

/// <summary>
/// Endpoints de unidades de biblioteca. Recurso pai de livros, clientes e funcionários.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BibliotecasController : ControllerBase
{
    private readonly IRepository<BibliotecaEntity> _repository;

    public BibliotecasController(IRepository<BibliotecaEntity> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Lista todas as bibliotecas cadastradas.
    /// </summary>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BibliotecaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BibliotecaResponse>>> GetAll()
    {
        var bibliotecas = await _repository.GetAllAsync();
        return Ok(bibliotecas.Select(BibliotecaResponse.From));
    }

    /// <summary>
    /// Busca uma biblioteca pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da biblioteca.</param>
    /// <response code="200">Biblioteca encontrada.</response>
    /// <response code="400">Identificador inválido.</response>
    /// <response code="404">Biblioteca não encontrada.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BibliotecaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BibliotecaResponse>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O identificador informado é inválido.");

        var biblioteca = await _repository.GetByIdAsync(id)
                         ?? throw new ResourceNotFoundException("Biblioteca", id);

        return Ok(BibliotecaResponse.From(biblioteca));
    }

    /// <summary>
    /// Cadastra uma nova unidade de biblioteca.
    /// </summary>
    /// <param name="request">Nome e endereço.</param>
    /// <response code="201">Biblioteca criada.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BibliotecaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BibliotecaResponse>> Create([FromBody] CreateBibliotecaRequest request)
    {
        var biblioteca = new BibliotecaEntity(request.Nome, request.Endereco);
        await _repository.AddAsync(biblioteca);

        var response = BibliotecaResponse.From(biblioteca);
        return CreatedAtAction(nameof(GetById), new { id = response.IdBiblioteca }, response);
    }
}
