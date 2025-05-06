using AutoMapper;
using backend.BancoDeDados;
using backend.Exceptions;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class UsuarioRepositorio
		(TarefasDbContext context, IMapper mapper) : IUsuarioRepositorio
	{
		public async Task<UsuarioResponse> BuscarUsuarioPorId(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException("Conta não foi encontrada");
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task<UsuarioResponse> CriarConta(UsuarioCreate usuario)
		{
			Usuario novaConta = mapper.Map<Usuario>(usuario);
			context.Usuarios.Add(novaConta);
			await context.SaveChangesAsync();
			return mapper.Map<UsuarioResponse>(novaConta);
		}

		public async Task<UsuarioResponse> EditarConta(Guid id, 
			UsuarioUpdate conta)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException("Conta não foi encontrada");
			mapper.Map(conta, usuario);
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task<UsuarioResponse> EntrarNaConta(UsuarioLogin conta)
		{
			Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(prop =>
				prop.Email == conta.Email && prop.Senha == conta.Senha) ??
				 throw new LoginErradoException("Email ou senha estão");
			return mapper.Map<UsuarioResponse>(usuario);
		}

		public async Task ExcluirConta(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException("Conta não foi encontrada");
			context.Remove(usuario);
			await context.SaveChangesAsync();
			return;
		}
	}
}
