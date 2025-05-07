using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;
using backend.Models.Tokens;

namespace backend.Repositorios.Interface
{
	public interface IUsuarioRepositorio
	{
		public Task<UsuarioResponse> BuscarUsuarioPorId(Guid id);
		public Task<TokenResponse> EntrarNaConta(UsuarioLogin conta);
		public Task<UsuarioResponse> EditarConta(Guid id, UsuarioUpdate conta);
		public Task<UsuarioResponse> CriarConta(UsuarioCreate usuario);
		public Task ExcluirConta(Guid id);
	}
}
