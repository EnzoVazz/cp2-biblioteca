using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Biblioteca : BaseEntity
{
    public string Nome { get; private set; }
    public string Endereco { get; private set; }
    
    public Guid IdBiblioteca { get; private set; } = Guid.NewGuid();
    
    // 1..N (Uma biblioteca tem vários funcionários)
    public List<Funcionario> Funcionarios { get; private set; } = new List<Funcionario>();

    // 1..N (Uma biblioteca tem vários livros)
    public List<Livro> Livros { get; private set; } = new List<Livro>();

    // 1..N (Uma biblioteca tem vários clientes)
    public List<Cliente> Clientes { get; private set; } = new List<Cliente>();
    

    protected Biblioteca() { }
    
    public Biblioteca(string nome, string endereco)
    {
        UpdateNome(nome);
        UpdateEndereco(endereco);
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome da biblioteca não pode ser vazio.");
        
        Nome = nome.Trim();
    }

    public void UpdateEndereco(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco) || endereco.Length < 5)
            throw new Exception("Endereço inválido. Deve conter pelo menos 5 caracteres.");
        
        Endereco = endereco;
    }
}