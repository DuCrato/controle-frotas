using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Moq;

namespace Fleet.Application.Tests.Condutores;

public class DeleteCondutorHandlerTests
{
    [Fact]
    public async Task Handle_DeveRemoverCondutor_QuandoEncontrado()
    {
        var condutor = CondutorTestFactory.Criar();
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(condutor);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new DeleteCondutorHandler(repository.Object);

        await handler.Handle(new DeleteCondutorCommand(condutor.Id), CancellationToken.None);

        repository.Verify(x => x.Deletar(condutor), Times.Once);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCondutorNaoEncontrado()
    {
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fleet.Domain.Condutores.Entidades.Condutor?)null);

        var handler = new DeleteCondutorHandler(repository.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(new DeleteCondutorCommand(Guid.NewGuid()), CancellationToken.None));

        repository.Verify(x => x.Deletar(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>()), Times.Never);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
