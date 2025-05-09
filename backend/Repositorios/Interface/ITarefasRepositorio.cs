using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Repositorios.Interface
{
	public interface ITarefasRepositorio
	{
		public Task<List<Tarefas>> BuscarTarefasPorUsuario(Guid idUsuario);
		public Task<Tarefas> CriarTarefa(Tarefas novaTarefa);

		public Task<Tarefas> AtualizarTarefa(Guid idTarefa, TarefasUpdate tarefa);

		public Task<Tarefas> AtualizarStatusTarefa(Guid idTarefa, bool status);
		public Task ExcluirTarefa(Guid idTarefa, Guid idUsuario);


	}
}
