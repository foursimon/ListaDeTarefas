using backend.BancoDeDados;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace backend.Repositorios
{
	public class UsuarioRepositorio
		(TarefasDbContext context) : IUsuarioRepositorio
	{
		public async Task<Usuario?> ArmazenarNovoUsuario(Usuario dados)
		{
			context.Usuarios.Add(dados);
			try
			{
				await context.SaveChangesAsync();
			//Usando um catch condicional, consigo verificar se o email já foi inserido
			}catch(DbUpdateException ex)
				when(ex.InnerException is MySqlException { Number: (int)MySqlErrorCode.DuplicateKeyEntry })
			{
				return null;
			}
			return dados;
		}

		public async Task<Usuario?> BuscarUsuarioPorEmail(string email)
		{
			return await context.Usuarios.FirstOrDefaultAsync(p => p.Email == email); ;
		}

		public async Task<Usuario?> BuscarUsuarioPorId(Guid id)
		{
			return await context.Usuarios.FindAsync(id);
		}

		public async Task DeletarUsuario(Usuario usuario)
		{
			context.Usuarios.Remove(usuario);
			await context.SaveChangesAsync();
			return;
		}

		public async Task<Usuario?> AtualizarUsuario(Usuario usuario)
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
				return null;
			}
			return usuario;
		}

		public async Task ExcluirTokenRecarga(Usuario usuario)
		{
			usuario.TokenRecarga = null;
			usuario.TempoToken = null;
			context.Usuarios.Update(usuario);
			await context.SaveChangesAsync();
			return;
		}
	}
}
