using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Models.Tokens;

namespace backend.Repositorios.Interface
{
	public interface IUsuarioRepositorio
	{
		public Task<Usuario> BuscarUsuarioPorId(Guid id);
		public Task<Usuario> BuscarUsuarioPorEmail(string email);
		public Task<Usuario> ArmazenarNovoUsuario(Usuario dados);

		public Task<Usuario> AtualizarUsuario(Usuario usuario);

		public Task DeletarUsuario(Guid id);
	}
}
