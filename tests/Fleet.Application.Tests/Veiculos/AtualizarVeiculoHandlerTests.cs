using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Enum;
using Moq;

namespace Fleet.Application.Tests.Veiculos;

public class AtualizarVeiculoHandlerTests
{
    [Fact]
    public async Task Handle_DeveAtualizarVeiculo_QuandoEncontrado()
    {
        var veiculo = VeiculoTestFactory.Criar();
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(veiculo);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new AtualizarVeiculoHandler(repository.Object);
        var command = new AtualizarVeiculoCommand(veiculo.Id, "Ana Costa", StatusVeiculoEnum.EmManutencao, "RJ", "Rio de Janeiro");

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Ana Costa", veiculo.NomeProprietario);
        Assert.Equal(StatusVeiculoEnum.EmManutencao, veiculo.Status);
        Assert.Equal("RJ", veiculo.Endereco.Estado);
        Assert.Equal("Rio de Janeiro", veiculo.Endereco.Cidade);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoVeiculoNaoEncontrado()
    {
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fleet.Domain.Veiculos.Entidades.Veiculo?)null);

        var handler = new AtualizarVeiculoHandler(repository.Object);
        var command = new AtualizarVeiculoCommand(Guid.NewGuid(), "Ana Costa", StatusVeiculoEnum.Ativo, "SP", "Sao Paulo");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
