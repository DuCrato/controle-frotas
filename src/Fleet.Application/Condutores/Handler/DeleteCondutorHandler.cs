using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Interface;
using MediatR;

namespace Fleet.Application.Condutores.Handler;

public sealed class DeleteCondutorHandler(ICondutorRepository repository) : IRequestHandler<DeleteCondutorCommand>
{
    public async Task Handle(DeleteCondutorCommand request, CancellationToken cancellationToken)
    {
        var condutor = await repository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Condutor não encontrado.");

        repository.Deletar(condutor);
        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}
