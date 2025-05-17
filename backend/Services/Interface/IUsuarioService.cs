using backend.Models.Dtos;

namespace backend.Services.Interface
{
	public interface IUsuarioService
	{
		public Task EntrarNaConta(UsuarioLogin conta);
		public Task<UsuarioResponse> BuscarInformacoesDoUsuario(); 
		public Task<UsuarioResponse> EditarConta(UsuarioUpdate conta);
		public Task<UsuarioResponse> CriarConta(UsuarioCreate usuario);

		public Task SairDaConta();
		public Task RecarregarToken();
		public Task ExcluirConta();
	}
}
