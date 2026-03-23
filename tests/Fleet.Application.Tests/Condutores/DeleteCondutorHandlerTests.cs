using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Tests.Common;

namespace Fleet.Application.Tests.Condutores;

public class DeleteCondutorHandlerTests : TestBase
{
    private readonly Mock<ICondutorRepository> _repositoryMock;
    private readonly DeleteCondutorHandler _handler;

    public DeleteCondutorHandlerTests()
    {
        _repositoryMock = new Mock<ICondutorRepository>();
        _handler = new DeleteCondutorHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenConductorFound_ShouldDeleteSuccessfully()
    {
        // Arrange
        var condutor = CondutorTestFactory.Criar();
        var command = new DeleteCondutorCommand(condutor.Id);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutor);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o condutor");

        _repositoryMock.Verify(
            x => x.Deletar(condutor),
            Times.Once,
            "deve deletar o condutor");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenConductorNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var conductorId = Guid.NewGuid();
        var command = new DeleteCondutorCommand(conductorId);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Condutores.Entidades.Condutor?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain(conductorId.ToString());

        _repositoryMock.Verify(
            x => x.Deletar(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>()),
            Times.Never,
            "não deve deletar condutor não encontrado");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve salvar alterações para condutor não encontrado");
    }

    [Fact]
    public async Task Handle_WhenSaveChangesFails_ShouldThrowException()
    {
        // Arrange
        var condutor = CondutorTestFactory.Criar();
        var command = new DeleteCondutorCommand(condutor.Id);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutor);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Database error", new Exception()));

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.Deletar(condutor),
            Times.Once,
            "deve tentar deletar mesmo se salvar falhar");
    }
}
