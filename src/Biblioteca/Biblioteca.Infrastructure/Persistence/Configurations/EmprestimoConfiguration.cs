using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.ToTable("T_EMPRESTIMO");
        
        builder.HasKey(e => e.IdEmprestimo);

        builder.Property(e => e.DataEmprestimo).IsRequired();
        
        builder.Property(e => e.DataDevolucao).IsRequired(false);

        builder.HasOne(e => e.Cliente)
            .WithMany(c => c.Emprestimos)
            .HasForeignKey(e => e.IdCliente);
    }
}