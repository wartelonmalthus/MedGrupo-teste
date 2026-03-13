using MedGrupo_teste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedGrupo_teste.Infraestructure.Persistence.Configurations;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(contact => contact.Id);

        builder.Property(contact => contact.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(contact => contact.BirthDate)
            .IsRequired();

        builder.Property(contact => contact.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contact => contact.IsActive)
            .IsRequired();

        builder.Property(contact => contact.CreatedAtUtc)
            .IsRequired();

        builder.Property(contact => contact.UpdatedAtUtc);

        builder.HasIndex(contact => new { contact.Name, contact.IsActive });
    }
}
