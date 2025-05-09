using backend.Exceptions.TarefasException;
using backend.Exceptions.UsuarioException;
using backend.Models.Dtos;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TarefasController(ITarefasService tarefasService) : ControllerBase
	{
		[HttpGet]
		[Authorize]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<TarefasResponse>> BuscarTarefasPorUsuario()
		{
			try
			{
				var resposta = await tarefasService.BuscarTarefasPorUsuario();
				if (resposta is null) return NoContent();
				return Ok(resposta);
			}
			catch (Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}

		[HttpPost]
		[Authorize]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> CriarNovaTarefa(TarefasCreate dados)
		{
			try
			{
				var resposta = await tarefasService.CriarNovaTarefa(dados);
				return Created("", resposta);
			}
			catch (QtdTarefaExcedidaException ex)
			{
				return BadRequest(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/400",
					Title = "Quantidade máxima de tarefas excedida",
					Detail = ex.Message,
					Status = StatusCodes.Status400BadRequest
				});
			}
			catch(UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Usuário não encontrado",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}

		[HttpPatch("{idTarefa}")]
		[Authorize]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> EditarTarefa(Guid idTarefa, TarefasUpdate dados)
		{
			try
			{
				var resposta = await tarefasService.EditarTarefa(idTarefa, dados);
				return Ok(resposta);
			}
			catch (TarefaNaoEncontradaException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Tarefa não foi encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}

		[HttpPatch("status/{idTarefa}")]
		[Authorize]
		[ProducesResponseType<TarefasResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TarefasResponse>> EditarTarefa(Guid idTarefa, [FromBody] bool status)
		{
			try
			{
				var resposta = await tarefasService.AtualizarStatusTarefa(idTarefa, status);
				return Ok(resposta);
			}
			catch (TarefaNaoEncontradaException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Tarefa não foi encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}

		[HttpDelete("{idTarefa}")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> ExcluirTarefa(Guid idTarefa)
		{
			try
			{
				await tarefasService.ExcluirTarefa(idTarefa);
				return NoContent();
			}
			catch (TarefaNaoEncontradaException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Tarefa não foi encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Usuário não encontrado",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}
	}
}
