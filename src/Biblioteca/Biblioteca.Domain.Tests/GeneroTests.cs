using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Exceptions;
using Xunit;

namespace Biblioteca.Domain.Tests;

public class GeneroTests
{
    [Fact]
    public void Construtor_QuandoNomeEDescricaoValidos_DeveCriarGeneroAtivo()
    {
        // Arrange
        const string nome = "Fantasia";
        const string descricao = "Narrativas com elementos magicos e mundos imaginarios.";

        // Act
        var genero = new Genero(nome, descricao);

        // Assert
        Assert.Equal("Fantasia", genero.Nome);
        Assert.Equal(descricao, genero.Descricao);
        Assert.True(genero.Active);
        Assert.NotEqual(Guid.Empty, genero.IdGenero);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("curta")]
    public void UpdateDescricao_QuandoDescricaoInvalida_DeveLancarDomainException(string? descricao)
    {
        // Arrange
        var genero = new Genero("Fantasia", "Descricao valida com mais de dez caracteres.");

        // Act
        var act = () => genero.UpdateDescricao(descricao!);

        // Assert
        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("A descrição deve conter pelo menos 10 caracteres.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateNome_QuandoNomeVazio_DeveLancarDomainException(string? nome)
    {
        // Arrange
        var genero = new Genero("Fantasia", "Descricao valida com mais de dez caracteres.");

        // Act
        var act = () => genero.UpdateNome(nome!);

        // Assert
        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("O nome do gênero não pode ser vazio.", exception.Message);
    }
}
