using backend.Infraestrutura;
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
			var resposta = await itemService.AdicionarItensNaLista(idTarefa, dados);
			if (resposta.IsSuccess) return Created("", resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
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
			var resposta = await itemService.EditarItens(idTarefa, dados);
			if (resposta.IsSuccess) return Ok(resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}
	}
}
