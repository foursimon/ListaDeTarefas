using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services.Interface;
using backend.Exceptions.UsuarioException;
using backend.Models.Dtos;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
	{

		[HttpPost("registrar")]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> CriarConta(UsuarioCreate dados)
		{
			try
			{
				var resposta = await usuarioService.CriarConta(dados);
				return Created("", resposta);
			}
			catch(EmailJaExisteException ex)
			{
				return BadRequest(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/400",
					Title = "Conta já existe",
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

		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> EntrarNaConta(UsuarioLogin dados)
		{
			try
			{
				var resposta = await usuarioService.EntrarNaConta(dados);
				usuarioService.ColocarTokensNoCookie(resposta);
				return Ok();
			}catch(LoginErradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Conta não encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}catch(Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
		}

		[HttpPost("recarregar-token")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TokenResponse>> RecarregarToken()
		{
			try
			{
				var resposta = await usuarioService.RecarregarToken();
				usuarioService.ColocarTokensNoCookie(resposta);
				return Ok();
			}
			catch (UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Conta não encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}
			catch(TokenInvalidoException ex)
			{
				return BadRequest(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/400",
					Title = "Informações inválidas",
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

		[HttpPatch]
		[Authorize]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> EditarConta(UsuarioUpdate dados)
		{
			try
			{
				var resposta = await usuarioService.EditarConta(dados);
				return Ok(resposta);
			}catch(UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Conta não encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
				});
			}catch(Exception ex)
			{
				return Problem(
					type: "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
					title: "Algo inesperado aconteceu.",
					detail: ex.Message,
					statusCode: StatusCodes.Status500InternalServerError
				);
			}
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
			try
			{
				await usuarioService.ExcluirConta();
				return NoContent();
			}catch(UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Conta não encontrada",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound
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

	}
}
