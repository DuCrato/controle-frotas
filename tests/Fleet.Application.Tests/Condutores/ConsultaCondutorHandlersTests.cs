using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Condutores.Query;
using Fleet.Application.Tests.Common;

namespace Fleet.Application.Tests.Condutores;

public class ConsultaCondutorHandlersTests : TestBase
{
    private readonly Mock<ICondutorRepository> _repositoryMock;

    public ConsultaCondutorHandlersTests()
    {
        _repositoryMock = new Mock<ICondutorRepository>();
    }

    [Fact]
    public async Task ObterCondutorPorIdHandler_WhenConductorExists_ShouldReturnConductorDto()
    {
        // Arrange
        var condutor = CondutorTestFactory.Criar();
        var query = new ObterCondutorPorIdQuery(condutor.Id);
        var handler = new ObterCondutorPorIdHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutor);

        // Act
        var dto = await handler.Handle(query, CancellationToken.None);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(condutor.Id);
        dto.Cpf.Should().Be(condutor.Cpf);

        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o condutor por ID");
    }

    [Fact]
    public async Task ObterCondutorPorIdHandler_WhenConductorNotExists_ShouldReturnNull()
    {
        // Arrange
        var conductorId = Guid.NewGuid();
        var query = new ObterCondutorPorIdQuery(conductorId);
        var handler = new ObterCondutorPorIdHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Condutores.Entidades.Condutor?)null);

        // Act
        var dto = await handler.Handle(query, CancellationToken.None);

        // Assert
        dto.Should().BeNull();
    }

    [Fact]
    public async Task ListagemCondutoresHandler_WhenConductorsExist_ShouldReturnAllConductors()
    {
        // Arrange
        var condutores = new List<Fleet.Domain.Condutores.Entidades.Condutor>
        {
            CondutorTestFactory.Criar(),
            CondutorTestFactory.Criar("Ana Costa", "98765432100", "98765432199"),
            CondutorTestFactory.Criar("Carlos Santos", "55544433322", "55544433399")
        };

        var query = new ListagemCondutoresQuery();
        var handler = new ListagemCondutoresHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutores);

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().HaveCount(3);
        resultado.Should().AllSatisfy(x => x.Should().NotBeNull());

        _repositoryMock.Verify(
            x => x.ListagemAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar todos os condutores");
    }

    [Fact]
    public async Task ListagemCondutoresHandler_WhenNoConductorsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new ListagemCondutoresQuery();
        var handler = new ListagemCondutoresHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Fleet.Domain.Condutores.Entidades.Condutor>());

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeEmpty();
    }
}
