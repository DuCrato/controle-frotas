using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[Route("api/veiculos")]
public sealed class VeiculosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarVeiculoCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(BuscarPorId), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarVeiculoCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id da rota diferente do corpo.");

        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await mediator.Send(new DeleteVeiculoCommand(id));
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var veiculo = await mediator.Send(new ObterVeiculoPorIdQuery(id));
        return Ok(veiculo);
    }

    [HttpGet]
    public async Task<IActionResult> BuscarParaListagem()
    {
        var lista = await mediator.Send(new ListagemVeiculosQuery());
        return Ok(lista);
    }
}