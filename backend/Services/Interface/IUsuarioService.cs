using backend.Models.Dtos;
using backend.Resultados;

namespace backend.Services.Interface
{
	public interface IUsuarioService
	{
		public Task<Result<Unit, Error>> EntrarNaConta(UsuarioLogin conta);
		public Task<Result<UsuarioResponse, Error>> BuscarInformacoesDoUsuario(); 
		public Task<Result<UsuarioResponse, Error>> EditarConta(UsuarioUpdate conta);
		public Task<Result<UsuarioResponse, Error>> CriarConta(UsuarioCreate usuario);

		public Task<Result<Unit, Error>> SairDaConta();
		public Task<Result<Unit, Error>> RecarregarToken();
		public Task<Result<Unit, Error>> ExcluirConta();
	}
}
