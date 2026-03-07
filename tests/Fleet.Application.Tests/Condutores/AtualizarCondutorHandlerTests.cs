using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Domain.Condutores.Enum;
using Moq;

namespace Fleet.Application.Tests.Condutores;

public class AtualizarCondutorHandlerTests
{
    [Fact]
    public async Task Handle_DeveSuspenderCondutor_QuandoStatusForSuspenso()
    {
        var condutor = CondutorTestFactory.Criar(status: StatusCondutorEnum.Ativo);
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(condutor);
        repository.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new AtualizarCondutorHandler(repository.Object);
        var command = new AtualizarCondutorCommand(condutor.Id, StatusCondutorEnum.Suspenso);

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal(StatusCondutorEnum.Suspenso, condutor.Status);
        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCondutorNaoEncontrado()
    {
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fleet.Domain.Condutores.Entidades.Condutor?)null);

        var handler = new AtualizarCondutorHandler(repository.Object);
        var command = new AtualizarCondutorCommand(Guid.NewGuid(), StatusCondutorEnum.Ativo);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        repository.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_AoReativarCondutorComCnhVencida()
    {
        var dataVencida = DateTime.Today.AddDays(-1);
        var condutor = CondutorTestFactory.Criar(status: StatusCondutorEnum.Inativo, validade: dataVencida);

        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(condutor);

        var handler = new AtualizarCondutorHandler(repository.Object);
        var command = new AtualizarCondutorCommand(condutor.Id, StatusCondutorEnum.Ativo);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
