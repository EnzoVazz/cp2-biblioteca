using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.API.Controllers;

/// <summary>
/// Endpoints de clientes da biblioteca. A senha nunca é exposta nas respostas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IRepository<Cliente> _clienteRepository;
    private readonly IRepository<BibliotecaEntity> _bibliotecaRepository;

    public ClientesController(
        IRepository<Cliente> clienteRepository,
        IRepository<BibliotecaEntity> bibliotecaRepository)
    {
        _clienteRepository = clienteRepository;
        _bibliotecaRepository = bibliotecaRepository;
    }

    /// <summary>
    /// Lista todos os clientes cadastrados.
    /// </summary>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetAll()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        return Ok(clientes.Select(ClienteResponse.From));
    }

    /// <summary>
    /// Busca um cliente pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do cliente.</param>
    /// <response code="200">Cliente encontrado.</response>
    /// <response code="400">Identificador inválido.</response>
    /// <response code="404">Cliente não encontrado.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClienteResponse>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O identificador informado é inválido.");

        var cliente = await _clienteRepository.GetByIdAsync(id)
                      ?? throw new ResourceNotFoundException("Cliente", id);

        return Ok(ClienteResponse.From(cliente));
    }

    /// <summary>
    /// Cadastra um novo cliente vinculado a uma biblioteca existente.
    /// </summary>
    /// <remarks>O cliente deve ser maior de 18 anos. A senha é hasheada no domínio e não retorna na resposta.</remarks>
    /// <param name="request">Dados cadastrais e senha em texto plano.</param>
    /// <response code="201">Cliente criado.</response>
    /// <response code="400">Dados inválidos (e-mail, idade ou senha).</response>
    /// <response code="404">Biblioteca informada não existe.</response>
    /// <response code="409">Já existe um cliente com o mesmo e-mail.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClienteResponse>> Create([FromBody] CreateClienteRequest request)
    {
        var cliente = new Cliente(
            request.Nome,
            request.Email,
            request.DataNascimento,
            request.Senha,
            request.IdBiblioteca);

        if (!await _bibliotecaRepository.ExistsByIdAsync(request.IdBiblioteca))
            throw new ResourceNotFoundException("Biblioteca", request.IdBiblioteca);

        var existentes = await _clienteRepository.GetAllAsync();
        if (existentes.Any(c => c.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            throw new ConflictException("Já existe um cliente com este e-mail.");

        await _clienteRepository.AddAsync(cliente);

        var response = ClienteResponse.From(cliente);
        return CreatedAtAction(nameof(GetById), new { id = response.IdCliente }, response);
    }
}
