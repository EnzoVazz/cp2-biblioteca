using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Genero : BaseEntity
{
    public Guid IdGenero { get; private set; } = Guid.NewGuid();
    
    public string Nome { get; private set; }
    public string Descricao { get; private set; }

    // N..N (Um gênero possui vários livros)
    public List<Livro> Livros { get; private set; } = new List<Livro>();

    protected Genero() { }
    
    public Genero(string nome, string descricao)
    {
        UpdateNome(nome);
        UpdateDescricao(descricao);
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome do gênero não pode ser vazio.");
        
        Nome = nome.Trim();
    }

    public void UpdateDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao) || descricao.Length < 10)
            throw new Exception("A descrição deve conter pelo menos 10 caracteres.");
        
        Descricao = descricao.Trim();
    }
}