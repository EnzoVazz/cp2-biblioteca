using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Xunit;

namespace Biblioteca.Domain.Tests;

public class LivroTests
{
    [Fact]
    public void Construtor_QuandoDadosValidos_DeveCriarLivro()
    {
        // Arrange
        var idBiblioteca = Guid.NewGuid();
        var dataLancamento = new DateOnly(2020, 1, 15);

        // Act
        var livro = new Livro(
            "O Hobbit",
            "Terra-média",
            "Uma jornada inesperada pelo mundo.",
            dataLancamento,
            310,
            idBiblioteca);

        // Assert
        Assert.Equal("O Hobbit", livro.Titulo);
        Assert.Equal(310, livro.NPaginas);
        Assert.Equal(idBiblioteca, livro.IdBiblioteca);
        Assert.True(livro.Active);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void UpdateNPaginas_QuandoNumeroInvalido_DeveLancarDomainException(int nPaginas)
    {
        // Arrange
        var livro = new Livro(
            "O Hobbit",
            string.Empty,
            "Uma jornada inesperada pelo mundo.",
            new DateOnly(2020, 1, 15),
            310,
            Guid.NewGuid());

        // Act
        var act = () => livro.UpdateNPaginas(nPaginas);

        // Assert
        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("O número de páginas deve ser maior que zero.", exception.Message);
    }
}
