using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Application.Veiculos.Validator
{
    public sealed class DeleteVeiculoCommandValidator : AbstractValidator<DeleteVeiculoCommand>
    {
        public DeleteVeiculoCommandValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id do veículo é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O Id do veículo não pode ser vazio.");
        }
    }
}
