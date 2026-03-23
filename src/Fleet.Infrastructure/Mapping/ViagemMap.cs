using Fleet.Domain.Viagens.Entidades;
using Fleet.Domain.Viagens.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Mapping;

public sealed class ViagemMap : IEntityTypeConfiguration<Viagem>
{
    public void Configure(EntityTypeBuilder<Viagem> builder)
    {
        builder.ToTable("Viagens");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.VeiculoId)
            .IsRequired();

        builder.Property(v => v.CondutorId)
            .IsRequired();

        builder.Property(v => v.DataHoraPrevistaSaida)
            .IsRequired();

        builder.Property(v => v.DataHoraPrevistaChegada)
            .IsRequired();

        builder.Property(v => v.DataHoraRealSaida);

        builder.Property(v => v.DataHoraRealChegada);

        builder.Property(v => v.QuiliometragemInicial);

        builder.Property(v => v.QuiliometragemFinal);

        builder.Property(v => v.DistanciaEstimada)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(v => v.Observacoes)
            .HasMaxLength(1000);

        builder.Property(v => v.Status)
            .IsRequired();

        builder.Property(v => v.DataCriacao)
            .IsRequired();

        builder.OwnsOne(v => v.Origem, origem =>
        {
            origem.WithOwner();

            origem.Property(o => o.Latitude)
                .HasColumnName("OrigemLatitude")
                .IsRequired()
                .HasPrecision(9, 6);

            origem.Property(o => o.Longitude)
                .HasColumnName("OrigemLongitude")
                .IsRequired()
                .HasPrecision(9, 6);

            origem.Property(o => o.Endereco)
                .HasColumnName("OrigemEndereco")
                .IsRequired()
                .HasMaxLength(500);
        });

        builder.OwnsOne(v => v.Destino, destino =>
        {
            destino.WithOwner();

            destino.Property(d => d.Latitude)
                .HasColumnName("DestinoLatitude")
                .IsRequired()
                .HasPrecision(9, 6);

            destino.Property(d => d.Longitude)
                .HasColumnName("DestinoLongitude")
                .IsRequired()
                .HasPrecision(9, 6);

            destino.Property(d => d.Endereco)
                .HasColumnName("DestinoEndereco")
                .IsRequired()
                .HasMaxLength(500);
        });

        builder.HasIndex(v => v.VeiculoId);
        builder.HasIndex(v => v.CondutorId);
        builder.HasIndex(v => v.Status);
        builder.HasIndex(v => v.DataCriacao);
    }
}
