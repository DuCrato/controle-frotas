using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Domain.Condutores.Enum;
using Moq;

namespace Fleet.Application.Tests.Condutores;

public class CriarCondutorHandlerTests
{
    [Fact]
    public async Task Handle_DeveCriarCondutor_QuandoCpfForUnico()
    {
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CriarCondutorHandler(repository.Object);
        var command = new CriarCondutorCommand(
            "Joao Silva",
            "12345678901",
            "12345678900",
            "B",
            DateTime.Today.AddYears(2),
            StatusCondutorEnum.Ativo);

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        repository.Verify(x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCpfJaExistir()
    {
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new CriarCondutorHandler(repository.Object);
        var command = new CriarCondutorCommand(
            "Joao Silva",
            "12345678901",
            "12345678900",
            "B",
            DateTime.Today.AddYears(2),
            StatusCondutorEnum.Ativo);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        repository.Verify(x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()), Times.Never);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
