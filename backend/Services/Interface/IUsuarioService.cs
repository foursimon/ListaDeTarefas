using backend.Models.Dtos.UsuarioDto;
using backend.Models.Tokens;

namespace backend.Services.Interface
{
	public interface IUsuarioService
	{
		public Task<TokenResponse> EntrarNaConta(UsuarioLogin conta);
		public Task<UsuarioResponse> EditarConta(UsuarioUpdate conta);
		public Task<UsuarioResponse> CriarConta(UsuarioCreate usuario);
		public Task<TokenResponse> RecarregarToken(Guid idUsuario, string tokenRecarga);
		public Task ExcluirConta();
	}
}
