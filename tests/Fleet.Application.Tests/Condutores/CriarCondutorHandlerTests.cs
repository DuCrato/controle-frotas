using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Tests.Common;
using Fleet.Domain.Condutores.Enum;

namespace Fleet.Application.Tests.Condutores;

public class CriarCondutorHandlerTests : TestBase
{
    private readonly Mock<ICondutorRepository> _repositoryMock;
    private readonly CriarCondutorHandler _handler;

    public CriarCondutorHandlerTests()
    {
        _repositoryMock = new Mock<ICondutorRepository>();
        _handler = new CriarCondutorHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCpfIsUnique_ShouldCreateConductorSuccessfully()
    {
        // Arrange
        var command = new CriarCondutorCommand(
            Fixture.Create<string>(),
            "12345678901",
            "12345678900",
            "B",
            DateTime.Today.AddYears(2),
            StatusCondutorEnum.Ativo);

        _repositoryMock
            .Setup(x => x.ExisteCpfAsync(command.Cpf, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var id = await _handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBe(Guid.Empty);

        _repositoryMock.Verify(
            x => x.ExisteCpfAsync(command.Cpf, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se CPF já existe");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()),
            Times.Once,
            "deve criar o condutor");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenCpfAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var cpf = "12345678901";
        var command = new CriarCondutorCommand(
            Fixture.Create<string>(),
            cpf,
            "12345678900",
            "B",
            DateTime.Today.AddYears(2),
            StatusCondutorEnum.Ativo);

        _repositoryMock
            .Setup(x => x.ExisteCpfAsync(cpf, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("CPF");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar condutor com CPF duplicado");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve salvar alterações com CPF duplicado");
    }

    [Fact]
    public async Task Handle_WhenSaveChangesFailsAsync_ShouldThrowException()
    {
        // Arrange
        var command = new CriarCondutorCommand(
            Fixture.Create<string>(),
            "12345678901",
            "12345678900",
            "B",
            DateTime.Today.AddYears(2),
            StatusCondutorEnum.Ativo);

        _repositoryMock
            .Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Condutores.Entidades.Condutor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Database error", new Exception()));

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
