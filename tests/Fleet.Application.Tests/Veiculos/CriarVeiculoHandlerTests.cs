using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Enum;
using Moq;

namespace Fleet.Application.Tests.Veiculos;

public class CriarVeiculoHandlerTests
{
    [Fact]
    public async Task Handle_DeveCriarVeiculo_QuandoDadosForemValidosEUnicos()
    {
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ExistePorPlacaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.ExistePorRenavamAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.ExistePorChassiAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CriarVeiculoHandler(repository.Object);
        var command = new CriarVeiculoCommand(
            "ABC1234",
            "12345678901",
            "9BWZZZ377VT004251",
            "Joao Silva",
            StatusVeiculoEnum.Ativo,
            "SP",
            "Sao Paulo");

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        repository.Verify(x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoPlacaJaExistir()
    {
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ExistePorPlacaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new CriarVeiculoHandler(repository.Object);
        var command = new CriarVeiculoCommand(
            "ABC1234",
            "12345678901",
            "9BWZZZ377VT004251",
            "Joao Silva",
            StatusVeiculoEnum.Ativo,
            "SP",
            "Sao Paulo");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        repository.Verify(x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()), Times.Never);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoRenavamJaExistir()
    {
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ExistePorPlacaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.ExistePorRenavamAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new CriarVeiculoHandler(repository.Object);
        var command = new CriarVeiculoCommand(
            "ABC1234",
            "12345678901",
            "9BWZZZ377VT004251",
            "Joao Silva",
            StatusVeiculoEnum.Ativo,
            "SP",
            "Sao Paulo");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        repository.Verify(x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()), Times.Never);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
