using AutoMapper;
using backend.BancoDeDados;
using backend.Exceptions;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Models.Security;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class UsuarioRepositorio
		(TarefasDbContext context, IMapper mapper, ISenhaHasher criptografia) : IUsuarioRepositorio
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

		public async Task<UsuarioResponse> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(prop =>
				prop.Email == conta.Email) ?? throw new LoginErradoException("Email ou senha incorretos");
			bool senhaCorreta = criptografia.VerificarSenha(conta.Senha, usuario.Senha);
			if (!senhaCorreta) throw new LoginErradoException("Email ou senha incorretos");
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task ExcluirConta(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException($"Conta com id {id} não foi encontrada");
			context.Remove(usuario);
			await context.SaveChangesAsync();
			return;
		}
	}
}
