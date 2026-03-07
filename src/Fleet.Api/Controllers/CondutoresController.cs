using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[Route("api/condutores")]
public sealed class CondutoresController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCondutorCommand command)
    {
        var id = await mediator.Send(command, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(BuscarPorId), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarCondutorCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id da rota diferente do corpo.");

        await mediator.Send(command, HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await mediator.Send(new DeleteCondutorCommand(id), HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var condutor = await mediator.Send(new ObterCondutorPorIdQuery(id), HttpContext.RequestAborted);
        return Ok(condutor);
    }

    [HttpGet]
    public async Task<IActionResult> BuscarParaListagem()
    {
        var lista = await mediator.Send(new ListagemCondutoresQuery(), HttpContext.RequestAborted);
        return Ok(lista);
    }
}
