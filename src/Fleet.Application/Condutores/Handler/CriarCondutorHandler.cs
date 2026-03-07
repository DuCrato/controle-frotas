using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Interface;
using Fleet.Domain.Condutores.Entidades;
using Fleet.Domain.Condutores.ValueObjects;
using MediatR;

namespace Fleet.Application.Condutores.Handler;

public sealed class CriarCondutorHandler(ICondutorRepository repository) : IRequestHandler<CriarCondutorCommand, Guid>
{
    public async Task<Guid> Handle(CriarCondutorCommand request, CancellationToken cancellationToken)
    {
        var cpfNormalizado = request.Cpf.Trim();

        if (await repository.ExisteCpfAsync(cpfNormalizado, cancellationToken))
            throw new InvalidOperationException($"Já existe um condutor cadastrado com este CPF: {cpfNormalizado}.");

        var cnh = new Cnh(request.CnhNumero, request.CnhCategoria, request.CnhValidade);

        var condutor = new Condutor(
            request.Nome,
            cpfNormalizado,
            cnh,
            request.Status);

        await repository.CriarAsync(condutor, cancellationToken);
        await repository.SalvarAlteracoesAsync(cancellationToken);

        return condutor.Id;
    }
}
