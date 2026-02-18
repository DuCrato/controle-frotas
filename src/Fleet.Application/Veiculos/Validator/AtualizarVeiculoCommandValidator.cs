using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Application.Veiculos.Validator
{
    public sealed class AtualizarVeiculoCommandValidator : AbstractValidator<AtualizarVeiculoCommand>
    {
        public AtualizarVeiculoCommandValidator()
        {
            RuleFor(x => x.NomeProprietario)
                .NotEmpty().WithMessage("O nome do proprietário é obrigatório.")
                .MaximumLength(150).WithMessage("O nome do proprietário deve conter no máximo 150 caracteres.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("O status é obrigatório.");
            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2).WithMessage("O estado deve conter no máximo 2 caracteres.");
            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .MinimumLength(3).WithMessage("A cidade deve conter no minimo 3 caracteres.");
        }
    }
}
