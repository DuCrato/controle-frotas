using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Application.Condutores.Validator;

public sealed class AtualizarCondutorCommandValidator : AbstractValidator<AtualizarCondutorCommand>
{
    public AtualizarCondutorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O Id do condutor é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O Id do condutor não pode ser vazio.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status do condutor é obrigatório.");
    }
}
