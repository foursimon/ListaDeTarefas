using backend.Models.Dtos;

namespace backend.Services.Interface
{
	public interface ITarefasService
	{
		public Task<List<TarefasResponse>> BuscarTarefasPorUsuario();

		public Task<TarefasResponse> CriarNovaTarefa(TarefasCreate dados);

		public Task<TarefasResponse> EditarTarefa(Guid idTarefa, TarefasUpdate dados);

		public Task<TarefasResponse> AtualizarStatusTarefa(Guid idTarefa, bool status);

		public Task ExcluirTarefa(Guid idTarefa);
	}
}
