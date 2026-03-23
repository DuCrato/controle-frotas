using Fleet.Application.Condutores.Command;
using FluentValidation;

namespace Fleet.Api.Validators.Condutores;

/// <summary>
/// Validador para o comando de criação de condutor.
/// </summary>
public class CriarCondutorCommandValidator : AbstractValidator<CriarCondutorCommand>
{
    public CriarCondutorCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(3)
            .WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(150)
            .WithMessage("Nome não pode exceder 150 caracteres");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("CPF é obrigatório")
            .Matches(@"^\d{11}$")
            .WithMessage("CPF deve conter 11 dígitos");

        RuleFor(x => x.CnhNumero)
            .NotEmpty()
            .WithMessage("Número de CNH é obrigatório")
            .Matches(@"^\d{11}$")
            .WithMessage("CNH deve conter 11 dígitos");

        RuleFor(x => x.CnhCategoria)
            .NotEmpty()
            .WithMessage("Categoria é obrigatória")
            .Must(c => new[] { "A", "B", "C", "D", "E", "AB", "AC", "AD", "AE" }.Contains(c))
            .WithMessage("Categoria inválida");

        RuleFor(x => x.CnhValidade)
            .NotEmpty()
            .WithMessage("Data de validade da CNH é obrigatória")
            .GreaterThan(DateTime.Today)
            .WithMessage("Data de validade deve ser superior a hoje");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status inválido");
    }
}

