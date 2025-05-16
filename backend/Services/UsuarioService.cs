using backend.Exceptions.UsuarioException;
using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Security;
using backend.Services.Interface;
using System.Security.Claims;

namespace backend.Services
{
	public class UsuarioService(IUsuarioRepositorio usuarioRepositorio, 
		ISenhaHasher criptografia, ITokenGerador geradorToken, 
		IHttpContextAccessor httpContext) : IUsuarioService
	{
		//Método responsável por armazenar informações em Cookies
		public void ColocarTokensNoCookie(TokenResponse tokens)
		{
			//Aqui estou armazenando o token de acesso no cookie
			httpContext.HttpContext!.Response.Cookies.Append("TOKEN_ACESSO", tokens.TokenAcesso,
				new CookieOptions
				{
					//Expires indica o tempo que o cookie irá expirar,
					Expires = DateTime.UtcNow.AddMinutes(10),
					//HttpOnly indica se o valor no cookie não pode ser acessado
					//por scripts no lado cliente (frontend)
					HttpOnly = true,
					//IsEssential indica que o valor que está sendo armazenado no cookie
					//é essencial para o funcionamento correto do sistema
					IsEssential = true,
					//Secure indica se o cookie só pode ser transmitido em conexão
					//HTTPS.
					Secure = true,
					//SameSite indica se o cookie pode ser usado entre sites
					SameSite = SameSiteMode.Strict
				});
			httpContext.HttpContext!.Response.Cookies.Append("TOKEN_RECARGA", tokens.TokenRecarga,
				new CookieOptions
				{
					Expires = DateTime.UtcNow.AddDays(7),
					HttpOnly = true,
					IsEssential = true,
					Secure = true,
					SameSite = SameSiteMode.Strict
				});

		}

		public async Task<UsuarioResponse> CriarConta(UsuarioCreate usuario)
		{
			//Antes de criar a conta do usuário, caso as informações enviadas
			//estejam válidas, eu criptografo a senha do usuário para
			//armazenar no banco de dados de forma segura.
			Usuario novaConta = usuario.ToUsuario();
			novaConta.Senha = criptografia.CriptografarSenha(novaConta.Senha);
			await usuarioRepositorio.ArmazenarNovoUsuario(novaConta);
			return novaConta.ToUsuarioResponse();
		}

		public async Task<UsuarioResponse> EditarConta(UsuarioUpdate conta)
		{
			Guid id = Guid.Parse(httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			Usuario usuarioEncontrado = await usuarioRepositorio.BuscarUsuarioPorId(id);
			Usuario usuario = conta.ToUsuario(usuarioEncontrado);
			if(conta.Senha is not null)
			{
				usuario.Senha = criptografia.CriptografarSenha(conta.Senha);
			}
			Usuario resposta = await usuarioRepositorio.AtualizarUsuario(usuario);
			return resposta.ToUsuarioResponse();
		}

		public async Task<TokenResponse> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario usuario = await usuarioRepositorio.BuscarUsuarioPorEmail(conta.Email);
			bool senhaCorreta = criptografia.VerificarSenha(conta.Senha, usuario.Senha);
			if (!senhaCorreta) throw new LoginErradoException("Email ou senha incorretos");
			//Um novo token de recarga é atribuído para o usuário quando
			//ele entra na sua conta manualmente.
			AtribuirTokenRecarga(usuario);
			var tokenAcesso = geradorToken.CriarToken(usuario.Nome,
				usuario.Id, usuario.Email);
			var tokenRecarga = usuario.TokenRecarga!;
			//Atualizo as informações salva no banco de dados
			//com o token de recarga criado.
			await usuarioRepositorio.AtualizarUsuario(usuario);
			return new TokenResponse(tokenAcesso, tokenRecarga);
		}

		public async Task ExcluirConta()
		{
			var id = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (id is null) throw new Exception("É necessário realizar login novamente");
			await usuarioRepositorio.DeletarUsuario(Guid.Parse(id));
			return;
		}
		public async Task<TokenResponse> RecarregarToken()
		{
			var tokenRecargaCookie = httpContext.HttpContext?.Request.Cookies["TOKEN_RECARGA"]
				?? throw new Exception("É necessário realizar login novamente");
			Guid idUsuario = geradorToken.PegarIdUsuarioToken(tokenRecargaCookie);
			Usuario usuario = await usuarioRepositorio.BuscarUsuarioPorId(idUsuario);
			TokenCriado tokenNovo = geradorToken.RecarregarToken(usuario, tokenRecargaCookie!);
			usuario.TokenRecarga = tokenNovo.TokenRecarga;
			usuario.TempoToken = tokenNovo.TempoToken;
			await usuarioRepositorio.AtualizarUsuario(usuario);
			var tokenAcesso = tokenNovo.TokenAcesso!;
			var tokenRecarga = tokenNovo.TokenRecarga;
			return new TokenResponse(tokenAcesso, tokenRecarga);
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
