using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Application.Condutores.Validator;

public sealed class CriarCondutorCommandValidator : AbstractValidator<CriarCondutorCommand>
{
    public CriarCondutorCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do condutor é obrigatório.")
            .MaximumLength(150).WithMessage("O nome do condutor deve conter no máximo 150 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF do condutor é obrigatório.")
            .Length(11).WithMessage("O CPF do condutor deve conter 11 caracteres.");

        RuleFor(x => x.CnhNumero)
            .NotEmpty().WithMessage("O número da CNH é obrigatório.");

        RuleFor(x => x.CnhCategoria)
            .NotEmpty().WithMessage("A categoria da CNH é obrigatória.")
            .MaximumLength(5).WithMessage("A categoria da CNH deve conter no máximo 5 caracteres.");

        RuleFor(x => x.CnhValidade)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("A CNH não pode estar vencida.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status do condutor é obrigatório.");
    }
}
