using backend.Models.Dtos;

namespace backend.Services.Interface
{
	public interface IUsuarioService
	{
		public Task<TokenResponse> EntrarNaConta(UsuarioLogin conta);
		public Task<UsuarioResponse> EditarConta(UsuarioUpdate conta);
		public Task<UsuarioResponse> CriarConta(UsuarioCreate usuario);
		public Task<TokenResponse> RecarregarToken();

		public void ColocarTokensNoCookie(TokenResponse tokens);
		public Task ExcluirConta();
	}
}
