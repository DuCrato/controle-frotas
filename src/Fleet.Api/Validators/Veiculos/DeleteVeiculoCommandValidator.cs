using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Veiculos;

/// <summary>
/// Validador para o comando de deletar veículo.
/// </summary>
public class DeleteVeiculoCommandValidator : AbstractValidator<DeleteVeiculoCommand>
{
    public DeleteVeiculoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID do veículo é obrigatório");
    }
}
