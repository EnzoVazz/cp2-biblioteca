using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("T_CLIENTE");
        
        builder.HasKey(c => c.IdCliente);

        builder.Property(c => c.Nome).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(150);
        builder.Property(c => c.DataNascimento).IsRequired();
        builder.Property(c => c.Senha).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Salt).IsRequired().HasMaxLength(100);

        builder.Ignore(c => c.Idade);
    }
}