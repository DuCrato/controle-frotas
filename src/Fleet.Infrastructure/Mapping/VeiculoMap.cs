using Fleet.Domain.Veiculos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Mapping;

public sealed class VeiculoMap : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("Veiculos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.NomeProprietario)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(v => v.Status)
            .IsRequired();

        builder.OwnsOne(v => v.Placa, placa =>
        {
            placa.WithOwner();

            placa.Property(p => p.Valor)
                .HasColumnName("Placa")
                .IsRequired()
                .HasMaxLength(7);

            placa.HasIndex(p => p.Valor).IsUnique();
        });

        builder.OwnsOne(v => v.Renavam, renavam =>
        {
            renavam.WithOwner();

            renavam.Property(r => r.Valor)
                .HasColumnName("Renavam")
                .IsRequired()
                .HasMaxLength(11);

            renavam.HasIndex(r => r.Valor).IsUnique();
        });

        builder.OwnsOne(v => v.Chassi, chassi =>
        {
            chassi.WithOwner();

            chassi.Property(c => c.Valor)
                .HasColumnName("Chassi")
                .IsRequired()
                .HasMaxLength(17);

            chassi.HasIndex(c => c.Valor).IsUnique();
        });

        builder.OwnsOne(v => v.Endereco, endereco =>
        {
            endereco.WithOwner();

            endereco.Property(e => e.Estado)
                .HasColumnName("Estado")
                .IsRequired()
                .HasMaxLength(2);

            endereco.Property(e => e.Cidade)
                .HasColumnName("Cidade")
                .IsRequired()
                .HasMaxLength(150);
        });
    }
}