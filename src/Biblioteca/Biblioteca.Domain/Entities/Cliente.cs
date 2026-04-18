using Biblioteca.Domain.Common;
using Biblioteca.Domain.Helpers;

namespace Biblioteca.Domain.Entities;

public class Cliente : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateOnly DataNascimento { get; private set; } 
    public string Senha { get; private set; }
    public string Salt { get; private set; } 
    
    public Guid IdCliente { get; private set; } = Guid.NewGuid();
    
    // N..1 (Vários clientes pertencem a 1 biblioteca)
    public Guid IdBiblioteca { get; private set; }
    public Biblioteca Biblioteca { get; private set; }

    // 1..N (Um cliente faz vários empréstimos)
    public List<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();
    
    protected Cliente() { }
    
    public Cliente(string nome, string email, DateOnly dataNascimento, string rawSenha, Guid idBiblioteca)
    {
        UpdateNome(nome);
        UpdateEmail(email);
        setDataNascimento(dataNascimento);
        MudarSenha(rawSenha);
        IdBiblioteca = idBiblioteca; 
    }

    public void UpdateNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new Exception("Nome não pode ser vazio.");
        
        Nome = novoNome;
    }

    public void UpdateEmail(string novoEmail)
    {
        if (string.IsNullOrWhiteSpace(novoEmail) || !novoEmail.Contains("@"))
            throw new Exception("E-mail inválido.");
        Email = novoEmail;
    }
    
    public void setDataNascimento(DateOnly novaDataNascimento)
    {
        var idade = CalculaIdade(novaDataNascimento);
        if (idade < 18)
            throw new Exception("O cliente deve ter mais que 18 anos.");
        DataNascimento = novaDataNascimento;
    }
    
    public void MudarSenha(string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
            throw new Exception("A senha deve conter pelo menos 6 caracteres.");
        Salt = Guid.NewGuid().ToString("N");
        
        Senha = HashHelper.Hash(novaSenha, Salt);
    }
    
    public int Idade => CalculaIdade(DataNascimento);

    private static int CalculaIdade(DateOnly date)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - date.Year;
        if (date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}