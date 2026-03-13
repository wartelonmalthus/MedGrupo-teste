using MedGrupo_teste.Application.DTOs;
using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedGrupo_teste.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContatosController : ControllerBase
{
    private readonly ServicoContato _servicoContato;

    public ContatosController(ServicoContato servicoContato)
    {
        _servicoContato = servicoContato;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResultadoPaginado<RespostaContato>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos([FromQuery] FiltroContato filtro, CancellationToken cancellationToken)
    {
        var contatos = await _servicoContato.ListarAsync(filtro, cancellationToken);
        return Ok(contatos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RespostaContato), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var contato = await _servicoContato.ObterAtivoPorIdAsync(id, cancellationToken);
            return Ok(contato);
        }
        catch (Exception exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(RespostaContato), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] RequisicaoContato requisicao, CancellationToken cancellationToken)
    {
        try
        {
            var contatoCriado = await _servicoContato.CriarAsync(requisicao, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = contatoCriado.Id }, contatoCriado);
        }
        catch (Exception exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(RespostaContato), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] RequisicaoAtualizacaoContato requisicao, CancellationToken cancellationToken)
    {
        try
        {
            var contatoAtualizado = await _servicoContato.AtualizarAsync(id, requisicao, cancellationToken);
            return Ok(contatoAtualizado);
        }
        catch (Exception exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _servicoContato.DesativarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (Exception exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _servicoContato.ExcluirAsync(id, cancellationToken);
            return NoContent();
        }
        catch (Exception exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
