using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaEntity = Biblioteca.Domain.Entities.Biblioteca;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class BibliotecaConfiguration : IEntityTypeConfiguration<BibliotecaEntity>
{
    public void Configure(EntityTypeBuilder<BibliotecaEntity> builder)
    {
        builder.ToTable("T_BIBLIOTECA");

        builder.HasKey(b => b.IdBiblioteca);

        builder.Property(b => b.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Endereco)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasMany(b => b.Funcionarios)
            .WithOne(f => f.Biblioteca)
            .HasForeignKey(f => f.IdBiblioteca);

        builder.HasMany(b => b.Livros)
            .WithOne(l => l.Biblioteca)
            .HasForeignKey(l => l.IdBiblioteca);

        builder.HasMany(b => b.Clientes)
            .WithOne(c => c.Biblioteca)
            .HasForeignKey(c => c.IdBiblioteca);
    }
}