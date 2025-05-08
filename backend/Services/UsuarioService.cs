using AutoMapper;
using backend.Exceptions;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Models.Tokens;
using backend.Repositorios.Interface;
using backend.Security;
using backend.Services.Interface;

namespace backend.Services
{
	public class UsuarioService(IMapper mapper, 
		IUsuarioRepositorio usuarioRepositorio, ISenhaHasher criptografia,
		ITokenGerador geradorToken) : IUsuarioService
	{
		public async Task<UsuarioResponse> CriarConta(UsuarioCreate usuario)
		{
			//Antes de criar a conta do usuário, caso as informações enviadas
			//estejam válidas, eu criptografo a senha do usuário para
			//armazenar no banco de dados de forma segura.
			usuario.Senha = criptografia.CriptografarSenha(usuario.Senha);
			Usuario novaConta = mapper.Map<Usuario>(usuario);
			await usuarioRepositorio.ArmazenarNovoUsuario(novaConta);
			return mapper.Map<UsuarioResponse>(novaConta);
		}

		public async Task<UsuarioResponse> EditarConta(Guid id,
			UsuarioUpdate conta)
		{
			if (conta.Senha is not null)
			{
				conta.Senha = criptografia.CriptografarSenha(conta.Senha);
			}
			Usuario usuario = await usuarioRepositorio.BuscarUsuarioPorId(id);
			mapper.Map(conta, usuario);
			Usuario resposta = await usuarioRepositorio.AtualizarUsuario(usuario);
			return mapper.Map<UsuarioResponse>(resposta);
		}

		public async Task<TokenResponse> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario usuario = await usuarioRepositorio.BuscarUsuarioPorEmail(conta.Email);
			bool senhaCorreta = criptografia.VerificarSenha(conta.Senha, usuario.Senha);
			if (!senhaCorreta) throw new LoginErradoException("Email ou senha incorretos");
			//Um novo token de recarga é atribuído para o usuário quando
			//ele entra na sua conta manualmente.
			AtribuirTokenRecarga(usuario);
			TokenResponse tokenResposta = new TokenResponse()
			{
				TokenAcesso = geradorToken.CriarToken(usuario.Nome,
					usuario.Id, usuario.Email),
				TokenRecarga = usuario.TokenRecarga!
			};
			//Atualizo as informações salva no banco de dados
			//com o token de recarga criado.
			await usuarioRepositorio.AtualizarUsuario(usuario);
			return tokenResposta;
		}

		public async Task ExcluirConta(Guid id)
		{
			await usuarioRepositorio.DeletarUsuario(id);
			return;
		}
		public async Task<TokenResponse> RecarregarToken(Guid idUsuario, string tokenRecarga)
		{
			Usuario usuario = await usuarioRepositorio.BuscarUsuarioPorId(idUsuario);

			TokenCriado tokenNovo = geradorToken.RecarregarToken(usuario, tokenRecarga);
			usuario.TokenRecarga = tokenNovo.TokenRecarga;
			usuario.TempoToken = tokenNovo.TempoToken;
			await usuarioRepositorio.AtualizarUsuario(usuario);
			return new TokenResponse()
			{
				TokenAcesso = tokenNovo.TokenAcesso!,
				TokenRecarga = tokenNovo.TokenRecarga
			};
		}
		//Método responsável por pegar o token de recarga
		//que é gerado através da classe TokenGerador.
		private void AtribuirTokenRecarga(Usuario usuario)
		{
			TokenCriado token = geradorToken.CriarTokenRecarga();
			//Estou atribuindo o token recebido para a conta de usuário
			//que for passada para este método.
			usuario.TokenRecarga = token.TokenRecarga;
			usuario.TempoToken = token.TempoToken;
		}
	}
}
