using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Application.Condutores.Validator;

public sealed class DeleteCondutorCommandValidator : AbstractValidator<DeleteCondutorCommand>
{
    public DeleteCondutorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O Id do condutor é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O Id do condutor não pode ser vazio.");
    }
}
