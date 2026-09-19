using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers;

/// <summary>
/// Endpoints de gêneros literários. Demonstra o CRUD completo via repositório genérico.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GenerosController : ControllerBase
{
    private readonly IRepository<Genero> _repository;
    private readonly ILogger<GenerosController> _logger;

    public GenerosController(IRepository<Genero> repository, ILogger<GenerosController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os gêneros cadastrados.
    /// </summary>
    /// <remarks>Leitura via repositório genérico, sem tracking do EF Core.</remarks>
    /// <response code="200">Lista de gêneros retornada com sucesso.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GeneroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GeneroResponse>>> GetAll()
    {
        var generos = await _repository.GetAllAsync();
        return Ok(generos.Select(GeneroResponse.From));
    }

    /// <summary>
    /// Busca um gênero pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do gênero.</param>
    /// <response code="200">Gênero encontrado.</response>
    /// <response code="400">Identificador inválido.</response>
    /// <response code="404">Gênero não encontrado.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeneroResponse>> GetById(Guid id)
    {
        EnsureValidId(id);

        var genero = await _repository.GetByIdAsync(id)
                     ?? throw new ResourceNotFoundException("Gênero", id);

        return Ok(GeneroResponse.From(genero));
    }

    /// <summary>
    /// Cadastra um novo gênero.
    /// </summary>
    /// <param name="request">Nome e descrição do gênero.</param>
    /// <response code="201">Gênero criado.</response>
    /// <response code="400">Dados inválidos (validação de domínio).</response>
    /// <response code="409">Já existe um gênero com o mesmo nome.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeneroResponse>> Create([FromBody] CreateGeneroRequest request)
    {
        _logger.LogInformation(
            "Iniciando cadastro de gênero. {TraceId} {Nome}",
            HttpContext.TraceIdentifier,
            request.Nome);

        var genero = new Genero(request.Nome, request.Descricao);
        await EnsureNomeUnico(request.Nome);
        await _repository.AddAsync(genero);

        _logger.LogInformation(
            "Gênero cadastrado com sucesso. {TraceId} {IdGenero} {Nome}",
            HttpContext.TraceIdentifier,
            genero.IdGenero,
            genero.Nome);

        var response = GeneroResponse.From(genero);
        return CreatedAtAction(nameof(GetById), new { id = response.IdGenero }, response);
    }

    /// <summary>
    /// Atualiza nome e descrição de um gênero existente.
    /// </summary>
    /// <param name="id">Identificador do gênero.</param>
    /// <param name="request">Novos dados.</param>
    /// <response code="200">Gênero atualizado.</response>
    /// <response code="400">Dados ou identificador inválidos.</response>
    /// <response code="404">Gênero não encontrado.</response>
    /// <response code="409">O novo nome já está em uso por outro gênero.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeneroResponse>> Update(Guid id, [FromBody] UpdateGeneroRequest request)
    {
        EnsureValidId(id);

        var genero = await _repository.GetByIdAsync(id)
                     ?? throw new ResourceNotFoundException("Gênero", id);

        await EnsureNomeUnico(request.Nome, id);

        genero.UpdateNome(request.Nome);
        genero.UpdateDescricao(request.Descricao);
        await _repository.UpdateAsync(genero);

        return Ok(GeneroResponse.From(genero));
    }

    /// <summary>
    /// Remove um gênero pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do gênero.</param>
    /// <response code="204">Gênero removido.</response>
    /// <response code="400">Identificador inválido.</response>
    /// <response code="404">Gênero não encontrado.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        EnsureValidId(id);

        var genero = await _repository.GetByIdAsync(id)
                     ?? throw new ResourceNotFoundException("Gênero", id);

        await _repository.DeleteAsync(genero);
        return NoContent();
    }

    private static void EnsureValidId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O identificador informado é inválido.");
    }

    private async Task EnsureNomeUnico(string nome, Guid? ignorarId = null)
    {
        var existentes = await _repository.GetAllAsync();
        var duplicado = existentes.Any(g =>
            g.Nome.Equals(nome?.Trim(), StringComparison.OrdinalIgnoreCase)
            && (!ignorarId.HasValue || g.IdGenero != ignorarId.Value));

        if (duplicado)
            throw new ConflictException("Já existe um gênero com este nome.");
    }
}
