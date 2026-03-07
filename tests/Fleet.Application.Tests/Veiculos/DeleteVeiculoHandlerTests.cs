using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Moq;

namespace Fleet.Application.Tests.Veiculos;

public class DeleteVeiculoHandlerTests
{
    [Fact]
    public async Task Handle_DeveRemoverVeiculo_QuandoEncontrado()
    {
        var veiculo = VeiculoTestFactory.Criar();
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(veiculo);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new DeleteVeiculoHandler(repository.Object);

        await handler.Handle(new DeleteVeiculoCommand(veiculo.Id), CancellationToken.None);

        repository.Verify(x => x.Delete(veiculo), Times.Once);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoVeiculoNaoEncontrado()
    {
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fleet.Domain.Veiculos.Entidades.Veiculo?)null);

        var handler = new DeleteVeiculoHandler(repository.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(new DeleteVeiculoCommand(Guid.NewGuid()), CancellationToken.None));

        repository.Verify(x => x.Delete(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>()), Times.Never);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
