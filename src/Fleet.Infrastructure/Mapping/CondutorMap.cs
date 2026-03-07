using Fleet.Domain.Condutores.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Mapping;

public sealed class CondutorMap : IEntityTypeConfiguration<Condutor>
{
    public void Configure(EntityTypeBuilder<Condutor> builder)
    {
        builder.ToTable("Condutores");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.Cpf)
            .IsUnique();

        builder.Property(c => c.Status)
            .IsRequired();

        builder.OwnsOne(c => c.Cnh, cnh =>
        {
            cnh.WithOwner();

            cnh.Property(x => x.Numero)
                .HasColumnName("CnhNumero")
                .IsRequired()
                .HasMaxLength(20);

            cnh.Property(x => x.Categoria)
                .HasColumnName("CnhCategoria")
                .IsRequired()
                .HasMaxLength(5);

            cnh.Property(x => x.Validade)
                .HasColumnName("CnhValidade")
                .IsRequired();
        });
    }
}
