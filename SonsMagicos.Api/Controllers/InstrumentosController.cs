﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonsMagicos.Api.Authorization;
using SonsMagicos.Aplicacao.DTOs;
using SonsMagicos.Aplicacao.Interfaces;


namespace SonsMagicos.Api.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class InstrumentosController : ControllerBase
{
    private IInstrumentoServico _instrumentoService;

    public InstrumentosController(IInstrumentoServico instrumentoService)
    {
        _instrumentoService = instrumentoService;
    }

    /// <summary>
    /// Obtém todos os Instrumentos
    /// </summary>
    /// <param name="propriedade">
    /// Filtra por propriedade os Instrumentos.<para />
    /// Por exemplo: <i>dormir</i>
    /// </param>
    /// <returns>Retorna lista de Instrumentos</returns>
    /// <response code="200">Retorna lista de Instrumentos</response>
    /// <response code="500">Erro no servidor</response>
    [AllowAnonymous]
    [HttpGet(Name = "GetTodosInstrumentos")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<InstrumentoDTO>>> Get(string? propriedade)
    {
        var instrumentos = await _instrumentoService.GetInstrumentosAsync(propriedade);
        return Ok(instrumentos);
    }

    /// <summary>
    /// Obtém Instrumento por Id
    /// </summary>
    /// <param name="id">Por exemplo: <i>1</i></param>
    /// <returns>Retorna um Instrumento</returns>
    /// <response code="200">Retorna um Instrumento</response>
    /// <response code="400">Requisição feita de forma incorreta</response>
    /// <response code="500">Erro no servidor</response>
    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetInstrumento")]
    [ProducesResponseType(typeof(InstrumentoDTO),200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<InstrumentoDTO>> GetPorId(int id)
    {
        var instrumento = await _instrumentoService.GetInstrumentoAsync(id);
        if (instrumento == null)
        {
            return NotFound();
        }
        return Ok(instrumento);
    }

    /// <summary>
    /// Cria um Instrumento
    /// </summary>
    /// <returns>Retorna o Instrumento criado</returns>
    /// <response code="201">Retorna o Instrumento criado</response>
    /// <response code="400">Requisição feita de forma incorreta</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="500">Erro no servidor</response>
    [HttpPost, BasicAuthorization]
    [ProducesResponseType(typeof(InstrumentoDTO),201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Post([FromBody] InstrumentoDTO instrumento)
    {
        if (instrumento == null)
        {
            return BadRequest();
        }
        await _instrumentoService.AddAsync(instrumento);

        return Created("", instrumento);
    }

    /// <summary>
    /// Atualiza um Instrumento
    /// </summary>
    /// <returns>Retorna o Instrumento atualizado</returns>
    /// <param name="id">Id do Instrumento</param>
    /// <response code="202">Retorna o Instrumento atualizado</response>
    /// <response code="400">Requisição feita de forma incorreta</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="404">Instrumento não encontrado</response>
    /// <response code="500">Erro no servidor</response>
    [HttpPut, BasicAuthorization]
    [ProducesResponseType(typeof(InstrumentoDTO), 202)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Put(int id, [FromBody] InstrumentoDTO instrumento)
    {

        if (id != instrumento.Id)
        {
            return BadRequest();
        }

        if (instrumento == null)
        {
            return BadRequest();
        }

        await _instrumentoService.UpdateAsync(instrumento);

        return Accepted(instrumento);
    }

    /// <summary>
    /// Deleta um Instrumento
    /// </summary>
    /// <returns>Retorna o Instrumento excluído</returns>
    /// <response code="202">Retorna o Instrumento excluído</response>
    /// <response code="400">Requisição feita de forma incorreta</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="404">Instrumento não encontrado</response>
    /// <response code="500">Erro no servidor</response>
    [HttpDelete("{id:int}"), BasicAuthorization]
    [ProducesResponseType(typeof(InstrumentoDTO), 202)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<InstrumentoDTO>> Delete(int id)
    {
        var instrumento = await _instrumentoService.GetInstrumentoAsync(id);
        if (instrumento == null)
        {
            return NotFound();
        }

        await _instrumentoService.RemoveAsync(id);
        return Accepted(instrumento);
    }

    /// <summary>
    /// Calcula o preço total de todos os Instrumentos de um tipo específico 
    /// </summary>
    /// <param name="tipo">Tipo dos Instrumentos</param>
    /// <returns>Retorna o preço total</returns>
    /// <response code="200">Retorna o preço total</response>
    /// <response code="400">Requisição feita de forma incorreta</response>
    /// <response code="500">Erro no servidor</response>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Route("ValorPorTipo")]
    public async Task<ActionResult<decimal>> GetPrecoPorTipo(TipoInstrumentoDTO tipo)
    {
        var total = await _instrumentoService.GetTotalValorPorTipoInstrumentoAsync(tipo);
        return Ok(total);
    }
}
