using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Infraestrutura.Mappers
{
	public static class TarefasMapperExtensions
	{
		public static TarefasResponse ToTarefasResponse(this Tarefas tarefa)
		{
			return new TarefasResponse(
				tarefa.Id,
				tarefa.Titulo,
				tarefa.Concluido,
				DateOnly.FromDateTime(tarefa.DataDeEncerramento),
				tarefa.Descricao,
				tarefa.Tipo,
				tarefa.Itens?.Select(p => p.ToCheckItemResponse()).ToList()
			);
		}
		public static Tarefas ToTarefas(this TarefasCreate tarefa, Guid idUsuario)
		{
			return new Tarefas
			{
				Titulo = tarefa.Titulo,
				Tipo = tarefa.Tipo,
				Concluido = false,
				Descricao = tarefa.Descricao,
				DataDeEncerramento = Convert.ToDateTime(tarefa.DataDeEncerramento.ToString() + " 00:00:00"),
				IdUsuario = idUsuario
			};
		}

		public static Tarefas ToTarefas(this TarefasUpdate dados, Tarefas tarefa)
		{
			tarefa.Titulo = dados.Titulo ?? tarefa.Titulo;
			tarefa.Tipo = dados.Tipo ?? tarefa.Tipo;
			tarefa.Descricao = dados.Descricao ?? tarefa.Descricao;
			if (dados.DataDeEncerramento != DateOnly.MinValue)
				tarefa.DataDeEncerramento = Convert.ToDateTime(dados.DataDeEncerramento.ToString() + " 00:00:00");
			return tarefa;
		}
	}
}
