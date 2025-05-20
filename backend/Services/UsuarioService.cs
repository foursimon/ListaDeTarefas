using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Resultados;
using backend.Security.Interface;
using backend.Services.Interface;
using System.Security.Claims;

namespace backend.Services
{
	public class UsuarioService(IUsuarioRepositorio usuarioRepositorio, 
		ISenhaHasher criptografia, ITokenGerador geradorToken, 
		IHttpContextAccessor httpContext, ICookies cookies) : IUsuarioService
	{
		public async Task<Result<UsuarioResponse, Error>> BuscarInformacoesDoUsuario()
		{
			var idUsuario = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (idUsuario is null) return UsuarioFailure.TokenInvalido;
			Usuario? usuario = await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(idUsuario!));
			if (usuario is null) return UsuarioFailure.UsuarioNaoEncontrado;
			return usuario.ToUsuarioResponse();
		}
		public async Task<Result<UsuarioResponse, Error>> CriarConta(UsuarioCreate usuario)
		{
			//Antes de criar a conta do usuário, caso as informações enviadas
			//estejam válidas, eu criptografo a senha do usuário para
			//armazenar no banco de dados de forma segura.
			Usuario novaConta = usuario.ToUsuario();
			novaConta.Senha = criptografia.CriptografarSenha(novaConta.Senha);
			Usuario? resposta = await usuarioRepositorio.ArmazenarNovoUsuario(novaConta);
			if (resposta is null) return UsuarioFailure.EmailJaExiste;
			return resposta.ToUsuarioResponse();
		}

		public async Task<Result<UsuarioResponse, Error>> EditarConta(UsuarioUpdate conta)
		{
			var id = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (id is null) return UsuarioFailure.UsuarioNaoEncontrado;
			Usuario? usuarioEncontrado = await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(id!));
			if (usuarioEncontrado is null) return UsuarioFailure.UsuarioNaoEncontrado;
			Usuario usuario = conta.ToUsuario(usuarioEncontrado);
			if(conta.Senha is not null)
			{
				usuario.Senha = criptografia.CriptografarSenha(conta.Senha);
			}
			Usuario? resposta = await usuarioRepositorio.AtualizarUsuario(usuario);
			if (resposta is null) return UsuarioFailure.EmailJaExiste;
			return resposta.ToUsuarioResponse();
		}

		public async Task<Result<Unit, Error>> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario? usuario = await usuarioRepositorio.BuscarUsuarioPorEmail(conta.Email);
			if (usuario is null) return UsuarioFailure.LoginErrado;
			bool senhaCorreta = criptografia.VerificarSenha(conta.Senha, usuario.Senha);
			if (!senhaCorreta) return UsuarioFailure.LoginErrado;
			//Um novo token de recarga é atribuído para o usuário quando
			//ele entra na sua conta manualmente.
			AtribuirTokenRecarga(usuario);
			var tokenAcesso = geradorToken.CriarToken(usuario.Nome,
				usuario.Id, usuario.Email);
			var tokenRecarga = usuario.TokenRecarga!;
			//Atualizo as informações salva no banco de dados
			//com o token de recarga criado.
			await usuarioRepositorio.AtualizarUsuario(usuario);
			cookies.ColocarTokensNoCookie(new TokenResponse(tokenAcesso, tokenRecarga),
				httpContext.HttpContext!);
			return Unit.Value;
		}
		public async Task<Result<Unit, Error>> SairDaConta()
		{
			var id = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (id is null) return UsuarioFailure.TokenInvalido;

			Usuario? usuario = await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(id));
			if (usuario is null) return UsuarioFailure.UsuarioNaoEncontrado;

			await usuarioRepositorio.ExcluirTokenRecarga(usuario);
			cookies.RemoverCookies(httpContext.HttpContext!);
			return Unit.Value;
		}
		public async Task<Result<Unit, Error>> ExcluirConta()
		{
			var id = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (id is null) return UsuarioFailure.TokenInvalido;

			Usuario? usuario = await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(id));
			if (usuario is null) return UsuarioFailure.UsuarioNaoEncontrado;
			
			await usuarioRepositorio.DeletarUsuario(usuario);
			cookies.RemoverCookies(httpContext.HttpContext!);
			return Unit.Value;
		}
		public async Task<Result<Unit, Error>> RecarregarToken()
		{
			var tokenRecargaCookie = httpContext.HttpContext?.Request.Cookies["TOKEN_RECARGA"];
			if (tokenRecargaCookie is null) return UsuarioFailure.TokenInvalido;

			var idUsuario = geradorToken.PegarIdUsuarioToken(tokenRecargaCookie);
			if (idUsuario is null) return UsuarioFailure.TokenInvalido;
			Usuario? usuario = await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(idUsuario));
			if (usuario is null) return UsuarioFailure.UsuarioNaoEncontrado;
			
			TokenCriado? tokenNovo = geradorToken.RecarregarToken(usuario, tokenRecargaCookie!);
			if (tokenNovo is null) return UsuarioFailure.TokenInvalido;
			usuario.TokenRecarga = tokenNovo.TokenRecarga;
			usuario.TempoToken = tokenNovo.TempoToken;

			await usuarioRepositorio.AtualizarUsuario(usuario);
			cookies.ColocarTokensNoCookie(new TokenResponse
				(tokenNovo.TokenAcesso!, tokenNovo.TokenRecarga),
				httpContext.HttpContext!);
			return Unit.Value;
		}

		//Método responsável por pegar o token de recarga
		//que é gerado através da classe TokenGerador.
		private void AtribuirTokenRecarga(Usuario usuario)
		{
			TokenCriado token = geradorToken.CriarTokenRecarga(usuario.Id);
			//Estou atribuindo o token recebido para a conta de usuário
			//que for passada para este método.
			usuario.TokenRecarga = token.TokenRecarga;
			usuario.TempoToken = token.TempoToken;
		}
	}
}
