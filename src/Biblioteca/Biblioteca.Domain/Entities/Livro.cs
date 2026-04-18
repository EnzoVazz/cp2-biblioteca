using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Livro : BaseEntity
{
    public Guid IdLivro { get; private set; } = Guid.NewGuid();
    
    public string Titulo { get; private set; }
    public string Serie { get; private set; }
    public string Descricao { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    public int NPaginas { get; private set; }

    // N..1 (Vários livros pertencem a 1 biblioteca)
    public Guid IdBiblioteca { get; private set; }
    public Biblioteca Biblioteca { get; private set; }

    // N..N (Um livro pode ter vários gêneros)
    public List<Genero> Generos { get; private set; } = new List<Genero>();

    // N..N (Um livro pode participar de vários empréstimos ao longo do tempo)
    public List<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();

    protected Livro() { }
    
    public Livro(string titulo, string serie, string descricao, DateOnly dataLancamento, int nPaginas, Guid idBiblioteca)
    {
        UpdateTitulo(titulo);
        Serie = serie ?? string.Empty; // Série pode ser vazia se o livro for um volume único
        UpdateDescricao(descricao);
        
        if (dataLancamento > DateOnly.FromDateTime(DateTime.Today))
            throw new Exception("A data de lançamento não pode ser no futuro.");
        DataLancamento = dataLancamento;

        UpdateNPaginas(nPaginas);
        
        IdBiblioteca = idBiblioteca;
    }

    public void UpdateTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("O título do livro não pode ser vazio.");
        
        Titulo = titulo.Trim();
    }

    public void UpdateDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new Exception("A descrição do livro não pode ser vazia.");
        
        Descricao = descricao.Trim();
    }

    public void UpdateNPaginas(int nPaginas)
    {
        if (nPaginas <= 0)
            throw new Exception("O número de páginas deve ser maior que zero.");
        
        NPaginas = nPaginas;
    }
}