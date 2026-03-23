using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Veiculos;

/// <summary>
/// Validador para o comando de atualização de veículo.
/// </summary>
public class AtualizarVeiculoCommandValidator : AbstractValidator<AtualizarVeiculoCommand>
{
    public AtualizarVeiculoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID do veículo é obrigatório");

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
