using backend.Models.Dtos;
using backend.Resultados;

namespace backend.Services.Interface
{
	public interface ITarefasService
	{
		public Task<Result<List<TarefasResponse>, Error>> BuscarTarefasPorUsuario();

		public Task<Result<TarefasResponse, Error>> CriarNovaTarefa(TarefasCreate dados);

		public Task<Result<TarefasResponse, Error>> EditarTarefa(Guid idTarefa, TarefasUpdate dados);

		public Task<Result<TarefasResponse, Error>> AtualizarStatusTarefa(Guid idTarefa, bool status);

		public Task<Result<Unit, Error>> ExcluirTarefa(Guid idTarefa);
	}
}
