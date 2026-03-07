using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Interface;
using Fleet.Domain.Condutores.Enum;
using MediatR;

namespace Fleet.Application.Condutores.Handler;

public sealed class AtualizarCondutorHandler(ICondutorRepository repository) : IRequestHandler<AtualizarCondutorCommand>
{
    public async Task Handle(AtualizarCondutorCommand request, CancellationToken cancellationToken)
    {
        var condutor = await repository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Condutor não encontrado.");

        if (request.Status == StatusCondutorEnum.Suspenso)
            condutor.Suspender();
        else if (request.Status == StatusCondutorEnum.Ativo)
            condutor.Reativar();
        else
            condutor.AtualizarStatus(request.Status);

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}
