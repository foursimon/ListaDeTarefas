using backend.Models.Entities;

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
