using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Application.Services;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Moq;
using Xunit;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.Application.Tests;

public class LivroAppServiceTests
{
    private readonly Mock<IRepository<Livro>> _livroRepository = new();
    private readonly Mock<IRepository<BibliotecaEntity>> _bibliotecaRepository = new();
    private readonly LivroAppService _sut;

    public LivroAppServiceTests()
    {
        _sut = new LivroAppService(_livroRepository.Object, _bibliotecaRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_QuandoBibliotecaNaoExiste_DeveLancarResourceNotFoundExceptionENaoPersistir()
    {
        // Arrange
        var request = CriarRequestValido();
        _bibliotecaRepository
            .Setup(r => r.ExistsByIdAsync(request.IdBiblioteca))
            .ReturnsAsync(false);

        // Act
        var act = () => _sut.CreateAsync(request);

        // Assert
        var exception = await Assert.ThrowsAsync<ResourceNotFoundException>(act);
        Assert.Equal("Biblioteca", exception.ResourceName);
        Assert.Equal(request.IdBiblioteca, exception.ResourceId);
        _livroRepository.Verify(r => r.AddAsync(It.IsAny<Livro>()), Times.Never);
        _livroRepository.Verify(r => r.UpdateAsync(It.IsAny<Livro>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_QuandoBibliotecaExiste_DevePersistirUmaVez()
    {
        // Arrange
        var request = CriarRequestValido();
        _bibliotecaRepository
            .Setup(r => r.ExistsByIdAsync(request.IdBiblioteca))
            .ReturnsAsync(true);

        // Act
        var response = await _sut.CreateAsync(request);

        // Assert
        Assert.Equal(request.Titulo, response.Titulo);
        Assert.Equal(request.IdBiblioteca, response.IdBiblioteca);
        Assert.NotEqual(Guid.Empty, response.IdLivro);
        _livroRepository.Verify(r => r.AddAsync(It.Is<Livro>(l =>
            l.Titulo == request.Titulo && l.IdBiblioteca == request.IdBiblioteca)), Times.Once);
    }

    private static CreateLivroRequest CriarRequestValido() => new()
    {
        Titulo = "O Hobbit",
        Serie = "Terra-média",
        Descricao = "Uma jornada inesperada pelo mundo.",
        DataLancamento = new DateOnly(1937, 9, 21),
        NPaginas = 310,
        IdBiblioteca = Guid.NewGuid()
    };
}
