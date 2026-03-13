using MedGrupo_teste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedGrupo_teste.Infraestructure.Persistence.Configurations;

public sealed class ConfiguracaoContato : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("Contatos");

        builder.HasKey(contato => contato.Id);

        builder.Property(contato => contato.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(contato => contato.DataNascimento)
            .IsRequired();

        builder.Property(contato => contato.Sexo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contato => contato.EstaAtivo)
            .IsRequired();

        builder.Property(contato => contato.CriadoEmUtc)
            .IsRequired();

        builder.Property(contato => contato.AtualizadoEmUtc);

        builder.HasIndex(contato => new { contato.Nome, contato.EstaAtivo });
    }
}
