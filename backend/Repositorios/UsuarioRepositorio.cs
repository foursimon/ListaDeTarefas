using AutoMapper;
using backend.BancoDeDados;
using backend.Exceptions;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Models.Tokens;
using backend.Repositorios.Interface;
using backend.Security;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class UsuarioRepositorio
		(TarefasDbContext context, IMapper mapper, ISenhaHasher criptografia,
		ITokenGerador geradorToken) : IUsuarioRepositorio
	{
		public async Task<UsuarioResponse> BuscarUsuarioPorId(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException($"Conta com id {id} não foi encontrada");
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task<UsuarioResponse> CriarConta(UsuarioCreate usuario)
		{
			//Antes de criar a conta do usuário, caso as informações enviadas
			//estejam válidas, eu criptografo a senha do usuário para
			//armazenar no banco de dados de forma segura.
			usuario.Senha = criptografia.CriptografarSenha(usuario.Senha);
			Usuario novaConta = mapper.Map<Usuario>(usuario);
			context.Usuarios.Add(novaConta);
			await context.SaveChangesAsync();
			return mapper.Map<UsuarioResponse>(novaConta);
		}

		public async Task<UsuarioResponse> EditarConta(Guid id, 
			UsuarioUpdate conta)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException($"Conta com id {id} não foi encontrada");
			if(conta.Senha is not null)
			{
				conta.Senha = criptografia.CriptografarSenha(conta.Senha);
			}
			mapper.Map(conta, usuario);
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task<TokenResponse> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(prop =>
				prop.Email == conta.Email) ?? throw new LoginErradoException("Email ou senha incorretos");
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
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return tokenResposta;
		}

		public async Task ExcluirConta(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException($"Conta com id {id} não foi encontrada");
			context.Remove(usuario);
			await context.SaveChangesAsync();
			return;
		}
		public async Task<TokenResponse> RecarregarToken(Guid idUsuario, string tokenRecarga)
		{
			Usuario usuario = await context.Usuarios.FindAsync(idUsuario)
				?? throw new UsuarioNaoEncontradoException($"Conta com id {idUsuario} não foi encontrada");

			TokenCriado tokenNovo = geradorToken.RecarregarToken(usuario, tokenRecarga);
			usuario.TokenRecarga = tokenNovo.TokenRecarga;
			usuario.TempoToken = tokenNovo.TempoToken;
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return new TokenResponse() { TokenAcesso = tokenNovo.TokenAcesso!, 
				TokenRecarga = tokenNovo.TokenRecarga };
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
