using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Emprestimo : BaseEntity
{
    // Atributos do seu diagrama
    public DateTime DataEmprestimo { get; private set; }
    public DateTime? DataDevolucao { get; private set; } 
    
    public Guid IdEmprestimo { get; private set; } = Guid.NewGuid();
    
    // N..1 (Vários empréstimos pertencem a 1 cliente)
    public Guid IdCliente { get; private set; } 
    public Cliente Cliente { get; private set; }

    // N..N (Um empréstimo pode conter vários livros)
    public List<Livro> Livros { get; private set; } = new List<Livro>();
    
    protected Emprestimo() { }
    
    public Emprestimo(Guid idCliente, DateTime dataEmprestimo)
    {
        IdCliente = idCliente;
        
        if (dataEmprestimo > DateTime.Now)
            throw new Exception("A data de empréstimo não pode ser no futuro.");
        
        DataEmprestimo = dataEmprestimo;
    }
    
    public void RegistrarDevolucao(DateTime dataDevolucao)
    {
        if (dataDevolucao < DataEmprestimo)
            throw new Exception("A data de devolução não pode ser anterior à data de empréstimo.");
        
        DataDevolucao = dataDevolucao;
    }
}