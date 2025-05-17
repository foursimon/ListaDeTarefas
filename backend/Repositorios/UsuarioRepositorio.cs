using backend.BancoDeDados;
using backend.Exceptions.UsuarioException;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace backend.Repositorios
{
	public class UsuarioRepositorio
		(TarefasDbContext context) : IUsuarioRepositorio
	{
		public async Task<Usuario> ArmazenarNovoUsuario(Usuario dados)
		{
			context.Usuarios.Add(dados);
			try
			{
				await context.SaveChangesAsync();
			//Usando um catch condicional, consigo verificar se o email já foi inserido
			}catch(DbUpdateException ex)
				when(ex.InnerException is MySqlException { Number: (int)MySqlErrorCode.DuplicateKeyEntry })
			{
				throw new EmailJaExisteException("O e-mail informado já está vinculado a uma conta");
			}
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
			try
			{
				await context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
				when (ex.InnerException is MySqlException { Number: (int)MySqlErrorCode.DuplicateKeyEntry })
			{
				throw new EmailJaExisteException("O e-mail informado já está vinculado a uma conta");
			}
			return usuario;
		}

		public async Task ExcluirTokenRecarga(Guid id)
		{
			Usuario usuario = await BuscarUsuarioPorId(id);
			usuario.TokenRecarga = null;
			usuario.TempoToken = null;
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return;
		}
	}
}
