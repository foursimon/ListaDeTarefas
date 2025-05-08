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
		(TarefasDbContext context) : IUsuarioRepositorio
	{
		public async Task<Usuario> ArmazenarNovoUsuario(Usuario dados)
		{
			context.Usuarios.Add(dados);
			await context.SaveChangesAsync();
			return dados;
		}

		public async Task<Usuario> BuscarUsuarioPorEmail(string email)
		{
			Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(p => p.Email == email) 
				?? throw new UsuarioNaoEncontradoException($"Usuário com email {email} não foi encontrado");
			return usuario;
		}

		public async Task<Usuario> BuscarUsuarioPorId(Guid id)
		{
			Usuario usuario = await context.Usuarios.FindAsync(id)
				?? throw new UsuarioNaoEncontradoException($"Usuario com id {id} não foi encontrado");
			return usuario;
		}

		public async Task DeletarUsuario(Guid id)
		{
			Usuario usuario = await BuscarUsuarioPorId(id);
			context.Usuarios.Remove(usuario);
			await context.SaveChangesAsync();
			return;
		}

		public async Task<Usuario> AtualizarUsuario(Usuario usuario)
		{
			await BuscarUsuarioPorId(usuario.Id);
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return usuario;
		}
	}
}
