using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable("T_FUNCIONARIO");
        
        builder.HasKey(f => f.IdFuncionario);

        builder.Property(f => f.Nome).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Email).IsRequired().HasMaxLength(150);
        builder.Property(f => f.DataNasc).IsRequired();
        builder.Property(f => f.Senha).IsRequired().HasMaxLength(255);
        builder.Property(f => f.Salt).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Turno).IsRequired().HasMaxLength(50);
    }
}