using backend.BancoDeDados;
using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Repositorios
{
	public class TarefasRepositorio(TarefasDbContext context) : ITarefasRepositorio
	{
		public async Task<Tarefas?> BuscarTarefaPorId(Guid idTarefa)
		{
			return await context.Tarefas.Include(p => p.Itens).FirstOrDefaultAsync
				(p => p.Id == idTarefa);
		}


		private void AtualizarQtdTarefas(Usuario usuario, bool criouTarefa)
		{
			if (criouTarefa) usuario.QuantidadeTarefa++;
			else usuario.QuantidadeTarefa--;
			context.Usuarios.Update(usuario);
		}
		public async Task<List<Tarefas>> BuscarTarefasPorUsuario(Guid idUsuario)
		{
			List<Tarefas> tarefas = await context.Tarefas.Where(p =>
				p.IdUsuario == idUsuario).Include(p => p.Itens).ToListAsync();
			return tarefas;
		}

		public async Task<Tarefas?> CriarTarefa(Tarefas novaTarefa, Usuario usuario)
		{
			if (usuario.QuantidadeTarefa >= 10) return null;
			context.AddRange(novaTarefa);
			AtualizarQtdTarefas(usuario, true);
			await context.SaveChangesAsync();
			return novaTarefa;
		}

		public async Task<Tarefas?> AtualizarTarefa(Tarefas tarefa, TarefasUpdate dados)
		{
			Tarefas tarefaAtualizada = dados.ToTarefas(tarefa);
			context.Tarefas.Update(tarefaAtualizada);
			await context.SaveChangesAsync();
			return tarefaAtualizada;
		}

		public async Task<Tarefas> AtualizarStatusTarefa(Tarefas tarefa, bool status)
		{
			tarefa.Concluido = status;
			context.Tarefas.Update(tarefa);
			await context.SaveChangesAsync();
			return tarefa;
		}


		public async Task ExcluirTarefa(Tarefas tarefa, Usuario usuario)
		{
			context.RemoveRange(tarefa);
			AtualizarQtdTarefas(usuario, false);
			await context.SaveChangesAsync();
			return;
		}
	}
}
