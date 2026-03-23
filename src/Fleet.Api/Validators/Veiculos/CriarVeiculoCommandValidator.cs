using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Veiculos;

/// <summary>
/// Validador para o comando de criação de veículo.
/// </summary>
public class CriarVeiculoCommandValidator : AbstractValidator<CriarVeiculoCommand>
{
    public CriarVeiculoCommandValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty()
            .WithMessage("Placa é obrigatória")
            .Matches(@"^[A-Z]{3}\d{4}$")
            .WithMessage("Placa deve estar no formato ABC1234");

        RuleFor(x => x.Renavam)
            .NotEmpty()
            .WithMessage("RENAVAM é obrigatório")
            .Matches(@"^\d{14}$")
            .WithMessage("RENAVAM deve conter 14 dígitos");

        RuleFor(x => x.Chassi)
            .NotEmpty()
            .WithMessage("Chassi é obrigatório")
            .Length(17)
            .WithMessage("Chassi deve conter 17 caracteres");

        RuleFor(x => x.NomeProprietario)
            .NotEmpty()
            .WithMessage("Nome do proprietário é obrigatório")
            .MinimumLength(3)
            .WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(150)
            .WithMessage("Nome não pode exceder 150 caracteres");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status inválido");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("Estado é obrigatório")
            .Length(2)
            .WithMessage("Estado deve conter 2 caracteres (UF)");

        RuleFor(x => x.Cidade)
            .NotEmpty()
            .WithMessage("Cidade é obrigatória")
            .MinimumLength(2)
            .WithMessage("Cidade deve ter no mínimo 2 caracteres");
    }
}
