using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;

namespace backend.Repositorios.Interface
{
	public interface IUsuarioRepositorio
	{
		public Task<UsuarioResponse> BuscarUsuarioPorId(Guid id);
		public Task<UsuarioResponse> EntrarNaConta(UsuarioLogin conta);
		public Task<UsuarioResponse> EditarConta(UsuarioUpdate conta);
		public Task<UsuarioResponse> CriarConta(UsuarioCreate usuario);
		public Task ExcluirConta(Guid id);
	}
}
