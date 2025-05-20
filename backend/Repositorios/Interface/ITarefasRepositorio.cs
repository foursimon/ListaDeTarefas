using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Repositorios.Interface
{
	public interface ITarefasRepositorio
	{
		public Task<List<Tarefas>> BuscarTarefasPorUsuario(Guid idUsuario);
		public Task<Tarefas?> BuscarTarefaPorId(Guid idTarefa);
		public Task<Tarefas?> CriarTarefa(Tarefas novaTarefa, Usuario usuario);

		public Task<Tarefas?> AtualizarTarefa(Tarefas tarefa, TarefasUpdate dados);

		public Task<Tarefas> AtualizarStatusTarefa(Tarefas tarefa, bool status);
		public Task ExcluirTarefa(Tarefas tarefa, Usuario usuario);


	}
}
