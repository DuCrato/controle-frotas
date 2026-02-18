using Fleet.Application.Veiculos.Command;
using FluentValidation;

namespace Fleet.Application.Veiculos.Validator
{
    public sealed class CriarVeiculoCommandValidator : AbstractValidator<CriarVeiculoCommand>
    {
        public CriarVeiculoCommandValidator()
        {
            RuleFor(x => x.Placa)
                .NotEmpty().WithMessage("A placa é obrigatória.")
                .Length(7).WithMessage("A placa deve conter no máximo 7 caracteres.");
            RuleFor(x => x.Renavam)
                .NotEmpty().WithMessage("O Renavam é obrigatório.")
                .Length(11).WithMessage("O Renavam deve conter no máximo 11 caracteres.");
            RuleFor(x => x.Chassi)
                .NotEmpty().WithMessage("O chassi é obrigatório.")
                .Length(17).WithMessage("O chassi deve conter no máximo 17 caracteres.");
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
