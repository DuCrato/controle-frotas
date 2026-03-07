using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Condutores.Query;
using Moq;

namespace Fleet.Application.Tests.Condutores;

public class ConsultaCondutorHandlersTests
{
    [Fact]
    public async Task ObterPorId_DeveRetornarDto_QuandoCondutorExistir()
    {
        var condutor = CondutorTestFactory.Criar();
        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(condutor);

        var handler = new ObterCondutorPorIdHandler(repository.Object);

        var dto = await handler.Handle(new ObterCondutorPorIdQuery(condutor.Id), CancellationToken.None);

        Assert.Equal(condutor.Id, dto.Id);
        Assert.Equal(condutor.Cpf, dto.Cpf);
    }

    [Fact]
    public async Task Listagem_DeveRetornarDtos_QuandoExistiremCondutores()
    {
        var condutores = new List<Fleet.Domain.Condutores.Entidades.Condutor>
        {
            CondutorTestFactory.Criar(),
            CondutorTestFactory.Criar("Ana Costa", "98765432100", "98765432100")
        };

        var repository = new Mock<ICondutorRepository>();
        repository.Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>())).ReturnsAsync(condutores);

        var handler = new ListagemCondutoresHandler(repository.Object);

        var resultado = await handler.Handle(new ListagemCondutoresQuery(), CancellationToken.None);

        Assert.Equal(2, resultado.Count);
    }
}
