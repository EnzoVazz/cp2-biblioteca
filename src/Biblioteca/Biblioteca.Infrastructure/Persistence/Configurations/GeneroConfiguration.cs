using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class GeneroConfiguration : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        builder.ToTable("T_GENERO");
        
        builder.HasKey(g => g.IdGenero);

        builder.Property(g => g.Nome).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Descricao).IsRequired().HasMaxLength(300);
    }
}