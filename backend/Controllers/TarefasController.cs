using backend.Infraestrutura;
using backend.Models.Dtos;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class TarefasController(ITarefasService tarefasService) : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> BuscarTarefasPorUsuario()
		{
			var resposta = await tarefasService.BuscarTarefasPorUsuario();
			if (!resposta.IsSuccess)
			{
				return StatusCode(resposta.Error!.CodigoStatus, 
					resposta.Error.ToProblemDetails());
			}
			if (resposta.Value!.Count == 0) return NoContent();
			return Ok(resposta.Value);
		}

		[HttpPost]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> CriarNovaTarefa(TarefasCreate dados)
		{
			var resposta = await tarefasService.CriarNovaTarefa(dados);
			if (resposta.IsSuccess) return Created("", resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPatch("{idTarefa}")]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> EditarTarefa(Guid idTarefa, TarefasUpdate dados)
		{
			var resposta = await tarefasService.EditarTarefa(idTarefa, dados);
			if (resposta.IsSuccess) return Ok(resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPatch("status/{idTarefa}")]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> AtualizarStatusTarefa(Guid idTarefa, [FromBody] bool status)
		{
			var resposta = await tarefasService.AtualizarStatusTarefa(idTarefa, status);
			if (resposta.IsSuccess) return Ok(resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpDelete("{idTarefa}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> ExcluirTarefa(Guid idTarefa)
		{
			var resposta = await tarefasService.ExcluirTarefa(idTarefa);
			if (resposta.IsSuccess) return NoContent();
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}
	}
}
