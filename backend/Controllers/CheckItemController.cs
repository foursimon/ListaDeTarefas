using backend.Exceptions.CheckItemException;
using backend.Exceptions.TarefasException;
using backend.Models.Dtos;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class CheckItemController(ICheckItemService itemService) : ControllerBase
	{
		[HttpPost("{idTarefa}")]
		[ProducesResponseType<CheckItemResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	
		public async Task<ActionResult<CheckItemResponse>> AdicionarItemNaLista
			(Guid idTarefa, List<CheckItemCreate> dados)
		{
			try{
				var resposta = await itemService.AdicionarItensNaLista(idTarefa, dados);
				return Created("", resposta);
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
			catch (ItemNaoExisteException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Item não foi encontrado",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch(ItensVaziosException ex)
			{
				return BadRequest(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/400",
					Title = "Não é possível adicionar itens vazios na lista",
					Detail = ex.Message,
					Status = StatusCodes.Status400BadRequest
				});
			}
			catch(Exception ex)
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
		[ProducesResponseType<CheckItemResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]

		public async Task<ActionResult<CheckItemResponse>> EditarItensDaLista
			(Guid idTarefa, List<CheckItemUpdate> dados)
		{
			try
			{
				var resposta = await itemService.EditarItens(idTarefa, dados);
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
			catch (ItemNaoExisteException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Item não foi encontrado",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch (ItensVaziosException ex)
			{
				return BadRequest(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/400",
					Title = "Não é possível adicionar itens vazios na lista",
					Detail = ex.Message,
					Status = StatusCodes.Status400BadRequest
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
