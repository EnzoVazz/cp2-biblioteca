using Biblioteca.Domain.Common;
using Biblioteca.Domain.Helpers;

namespace Biblioteca.Domain.Entities;

public class Funcionario : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateOnly DataNasc { get; private set; } 
    public string Senha { get; private set; }
    public string Salt { get; private set; } 
    public string Turno { get; private set; } 
    
    public Guid IdFuncionario { get; private set; } = Guid.NewGuid();
    
    // N..1 (Vários funcionários pertencem a 1 biblioteca)
    public Guid IdBiblioteca { get; private set; } 
    public Biblioteca Biblioteca { get; private set; }

    protected Funcionario() { }
    
    public Funcionario(string nome, string email, DateOnly dataNasc, string rawSenha, string turno, Guid idBiblioteca)
    {
        UpdateNome(nome);
        UpdateEmail(email);
        
        // Funcionário precisa ser maior de idade para trabalhar
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        if (dataNasc > hoje.AddYears(-18))
            throw new Exception("O funcionário deve ter pelo menos 18 anos.");
        DataNasc = dataNasc;

        MudarSenha(rawSenha);
        UpdateTurno(turno);
        
        IdBiblioteca = idBiblioteca; 
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome não pode ser vazio.");
        Nome = nome;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new Exception("E-mail inválido.");
        Email = email;
    }

    public void UpdateTurno(string turno)
    {
        if (string.IsNullOrWhiteSpace(turno))
            throw new Exception("Turno não pode ser vazio.");
        Turno = turno;
    }

    public void MudarSenha(string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
            throw new Exception("A senha deve conter pelo menos 6 caracteres.");
        
        Salt = Guid.NewGuid().ToString("N");
        Senha = HashHelper.Hash(novaSenha, Salt);
    }
}