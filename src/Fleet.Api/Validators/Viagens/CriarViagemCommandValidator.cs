using Fleet.Application.Viagens.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Viagens;

/// <summary>
/// Validador para o comando de criar uma viagem.
/// </summary>
public class CriarViagemCommandValidator : AbstractValidator<CriarViagemCommand>
{
    public CriarViagemCommandValidator()
    {
        RuleFor(x => x.VeiculoId)
            .NotEmpty()
            .WithMessage("ID do veículo é obrigatório");

        RuleFor(x => x.CondutorId)
            .NotEmpty()
            .WithMessage("ID do condutor é obrigatório");

        RuleFor(x => x.LatitudeOrigem)
            .GreaterThanOrEqualTo(-90m)
            .WithMessage("Latitude deve ser maior ou igual a -90")
            .LessThanOrEqualTo(90m)
            .WithMessage("Latitude deve ser menor ou igual a 90");

        RuleFor(x => x.LongitudeOrigem)
            .GreaterThanOrEqualTo(-180m)
            .WithMessage("Longitude deve ser maior ou igual a -180")
            .LessThanOrEqualTo(180m)
            .WithMessage("Longitude deve ser menor ou igual a 180");

        RuleFor(x => x.EnderecoOrigem)
            .NotEmpty()
            .WithMessage("Endereço de origem é obrigatório")
            .MinimumLength(5)
            .WithMessage("Endereço deve ter no mínimo 5 caracteres");

        RuleFor(x => x.LatitudeDestino)
            .GreaterThanOrEqualTo(-90m)
            .WithMessage("Latitude deve ser maior ou igual a -90")
            .LessThanOrEqualTo(90m)
            .WithMessage("Latitude deve ser menor ou igual a 90");

        RuleFor(x => x.LongitudeDestino)
            .GreaterThanOrEqualTo(-180m)
            .WithMessage("Longitude deve ser maior ou igual a -180")
            .LessThanOrEqualTo(180m)
            .WithMessage("Longitude deve ser menor ou igual a 180");

        RuleFor(x => x.EnderecoDestino)
            .NotEmpty()
            .WithMessage("Endereço de destino é obrigatório")
            .MinimumLength(5)
            .WithMessage("Endereço deve ter no mínimo 5 caracteres");

        RuleFor(x => x.DataHoraPrevistaSaida)
            .NotEmpty()
            .WithMessage("Data/hora prevista de saída é obrigatória")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Data de saída deve ser no futuro");

        RuleFor(x => x.DataHoraPrevistaChegada)
            .NotEmpty()
            .WithMessage("Data/hora prevista de chegada é obrigatória")
            .GreaterThan(x => x.DataHoraPrevistaSaida)
            .WithMessage("Data de chegada deve ser posterior à data de saída");

        RuleFor(x => x.DistanciaEstimada)
            .GreaterThan(0m)
            .WithMessage("Distância estimada deve ser maior que zero");
    }
}
