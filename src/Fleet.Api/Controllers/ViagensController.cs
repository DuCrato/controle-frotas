using Fleet.Application.Viagens.Command;
using Fleet.Application.Viagens.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[Route("api/viagens")]
public sealed class ViagensController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Cria uma nova viagem.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarViagemCommand command)
    {
        var id = await mediator.Send(command, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    /// <summary>
    /// Inicia uma viagem planejada.
    /// </summary>
    [HttpPost("{id:guid}/iniciar")]
    public async Task<IActionResult> Iniciar(Guid id, [FromBody] IniciarViagemBody body)
    {
        var command = new IniciarViagemCommand(id, body.QuiliometragemInicial);
        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Conclui uma viagem em andamento.
    /// </summary>
    [HttpPost("{id:guid}/concluir")]
    public async Task<IActionResult> Concluir(Guid id, [FromBody] ConcluirViagemBody body)
    {
        var command = new ConcluirViagemCommand(id, body.QuiliometragemFinal);
        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Pausa uma viagem em andamento.
    /// </summary>
    [HttpPost("{id:guid}/pausar")]
    public async Task<IActionResult> Pausar(Guid id)
    {
        var command = new PausarViagemCommand(id);
        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Retoma uma viagem pausada.
    /// </summary>
    [HttpPost("{id:guid}/retomar")]
    public async Task<IActionResult> Retomar(Guid id)
    {
        var command = new RetomarViagemCommand(id);
        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Cancela uma viagem.
    /// </summary>
    [HttpPost("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        var command = new CancelarViagemCommand(id);
        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Obtém uma viagem pelo ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var viagem = await mediator.Send(new ObterViagemPorIdQuery(id), HttpContext.RequestAborted);
        return Ok(viagem);
    }

    /// <summary>
    /// Lista todas as viagens.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var viagens = await mediator.Send(new ListagemViagensQuery(), HttpContext.RequestAborted);
        return Ok(viagens);
    }

    /// <summary>
    /// Lista viagens de um condutor específico.
    /// </summary>
    [HttpGet("condutor/{condutorId:guid}")]
    public async Task<IActionResult> ListarPorCondutor(Guid condutorId)
    {
        var viagens = await mediator.Send(new ListagemViagensPorCondutorQuery(condutorId), HttpContext.RequestAborted);
        return Ok(viagens);
    }

    /// <summary>
    /// Lista viagens de um veículo específico.
    /// </summary>
    [HttpGet("veiculo/{veiculoId:guid}")]
    public async Task<IActionResult> ListarPorVeiculo(Guid veiculoId)
    {
        var viagens = await mediator.Send(new ListagemViagensPorVeiculoQuery(veiculoId), HttpContext.RequestAborted);
        return Ok(viagens);
    }
}

public sealed record IniciarViagemBody(decimal QuiliometragemInicial);
public sealed record ConcluirViagemBody(decimal QuiliometragemFinal);
