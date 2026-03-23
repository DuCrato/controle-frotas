using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Condutores;

/// <summary>
/// Validador para o comando de atualização de condutor.
/// </summary>
public class AtualizarCondutorCommandValidator : AbstractValidator<AtualizarCondutorCommand>
{
    public AtualizarCondutorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID do condutor é obrigatório");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status inválido");
    }
}
