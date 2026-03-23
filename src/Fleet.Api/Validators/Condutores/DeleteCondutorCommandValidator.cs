using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Condutores;

/// <summary>
/// Validador para o comando de deletar condutor.
/// </summary>
public class DeleteCondutorCommandValidator : AbstractValidator<DeleteCondutorCommand>
{
    public DeleteCondutorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID do condutor é obrigatório");
    }
}
