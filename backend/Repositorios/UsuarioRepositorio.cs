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
			AtribuirTokenRecarga(usuario);
			TokenResponse tokenResposta = new TokenResponse()
			{
				TokenAcesso = geradorToken.CriarToken(usuario.Nome, 
					usuario.Id, usuario.Email),
				TokenRecarga = usuario.TokenRecarga!
			};
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

		private void AtribuirTokenRecarga(Usuario usuario)
		{
			TokenCriado token = geradorToken.CriarTokenRecarga();
			usuario.TokenRecarga = token.TokenRecarga;
			usuario.TempoToken = token.TempoToken;
		}

	}
}
