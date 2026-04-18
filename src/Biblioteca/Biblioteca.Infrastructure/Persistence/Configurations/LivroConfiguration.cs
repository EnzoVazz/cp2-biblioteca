using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("T_LIVRO");
        
        builder.HasKey(l => l.IdLivro);

        builder.Property(l => l.Titulo).IsRequired().HasMaxLength(200);
        builder.Property(l => l.Serie).HasMaxLength(100);
        builder.Property(l => l.Descricao).IsRequired().HasMaxLength(500);
        builder.Property(l => l.DataLancamento).IsRequired();
        builder.Property(l => l.NPaginas).IsRequired();

        builder.HasMany(l => l.Generos)
            .WithMany(g => g.Livros)
            .UsingEntity(j => j.ToTable("T_LIVRO_GENERO"));

        builder.HasMany(l => l.Emprestimos)
            .WithMany(e => e.Livros)
            .UsingEntity(j => j.ToTable("T_LIVRO_EMPRESTIMO"));
    }
}