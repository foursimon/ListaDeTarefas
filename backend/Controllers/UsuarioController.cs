using backend.Models.Entities;
using backend.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using backend.Repositorios.Interface;
using backend.Exceptions;
using backend.Models.Dtos.UsuarioDto;
using Microsoft.AspNetCore.Authorization;
using backend.Models.Tokens;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController(IUsuarioRepositorio usuarioRepositorio) : ControllerBase
	{
		[HttpGet("{id}")]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> BuscarUsuarioPorId(Guid id)
		{
			try
			{
				var resposta = await usuarioRepositorio.BuscarUsuarioPorId(id);
				return Ok(resposta);
			}
			catch (UsuarioNaoEncontradoException ex)
			{
				return NotFound(new ProblemDetails
				{
					Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/404",
					Title = "Usuário não encontrado",
					Detail = ex.Message,
					Status = StatusCodes.Status404NotFound,
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

		[HttpPost("registrar")]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> CriarConta(UsuarioCreate dados)
		{
			try
			{
				var resposta = await usuarioRepositorio.CriarConta(dados);
				return CreatedAtAction(nameof(
					BuscarUsuarioPorId), new {id = resposta.Id}, resposta);
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

		[HttpPost("login")]
		[ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TokenResponse>> EntrarNaConta(UsuarioLogin dados)
		{
			try
			{
				var resposta = await usuarioRepositorio.EntrarNaConta(dados);
				return Ok(resposta);
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

		[HttpPatch("{id}")]
		[Authorize]
		[ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<UsuarioResponse>> EditarConta(Guid id, UsuarioUpdate dados)
		{
			try
			{
				var resposta = await usuarioRepositorio.EditarConta(id, dados);
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

		[HttpDelete("{id}")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> ExclurConta(Guid id)
		{
			try
			{
				await usuarioRepositorio.ExcluirConta(id);
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
