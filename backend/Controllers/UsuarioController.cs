using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services.Interface;
using backend.Models.Dtos;
using backend.Infraestrutura;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> BuscarInformacoesDoUsuario()
		{
			var resposta = await usuarioService.BuscarInformacoesDoUsuario();
			if (resposta.IsSuccess) return Ok(resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPost("registrar")]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> CriarConta(UsuarioCreate dados)
		{
			var resposta = await usuarioService.CriarConta(dados);
			if (resposta.IsSuccess) return Created("", resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> EntrarNaConta(UsuarioLogin dados)
		{
			var resposta = await usuarioService.EntrarNaConta(dados);
			if (resposta.IsSuccess) return Ok();
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPost("recarregar-token")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> RecarregarToken()
		{
			var resposta = await usuarioService.RecarregarToken();
			if (resposta.IsSuccess) return Ok();
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpPatch]
		[Authorize]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> EditarConta(UsuarioUpdate dados)
		{
			var resposta = await usuarioService.EditarConta(dados);
			if (resposta.IsSuccess) return Ok(resposta.Value);
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpDelete]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> ExclurConta()
		{
			var resposta = await usuarioService.ExcluirConta();
			if (resposta.IsSuccess) return NoContent();
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

		[HttpDelete("logout")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> SairDaConta()
		{
			var resposta = await usuarioService.SairDaConta();
			if (resposta.IsSuccess) return NoContent();
			return StatusCode(resposta.Error!.CodigoStatus, resposta.Error.ToProblemDetails());
		}

	}
}
