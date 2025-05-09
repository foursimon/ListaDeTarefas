using backend.BancoDeDados;
using backend.Exceptions.TarefasException;
using backend.Exceptions.UsuarioException;
using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class TarefasRepositorio(TarefasDbContext context) : ITarefasRepositorio
	{
		private async Task<Tarefas> BuscarTarefaPorId(Guid id)
		{
			Tarefas tarefa = await context.Tarefas.FindAsync(id)
				?? throw new TarefaNaoEncontradaException
					($"Tarefa com id {id} não foi encontrada");
			return tarefa;
		}

		private async Task VerificarQtdTarefas(Guid idUsuario)
		{
			Usuario usuario = await context.Usuarios.FindAsync(idUsuario)
				?? throw new UsuarioNaoEncontradoException($"Usuário com o id {idUsuario} não foi encontrado");
			if (usuario.QuantidadeTarefa >=10)
				throw new QtdTarefaExcedidaException("Não é possível criar mais uma tarefa por ter exceido quantidade máxima");
			return;
		}

		private async Task AtualizarQtdTarefas(Guid idUsuario, bool criouTarefa)
		{
			Usuario usuario = await context.Usuarios.FindAsync(idUsuario)
				?? throw new UsuarioNaoEncontradoException($"Usuário com o id {idUsuario} não foi encontrado");
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

		public async Task<Tarefas> CriarTarefa(Tarefas novaTarefa)
		{
			await VerificarQtdTarefas(novaTarefa.IdUsuario);
			context.AddRange(novaTarefa);
			await AtualizarQtdTarefas(novaTarefa.IdUsuario, true);
			await context.SaveChangesAsync();
			return novaTarefa;
		}

		public async Task<Tarefas> AtualizarTarefa(Guid idTarefa, TarefasUpdate dados)
		{
			Tarefas tarefaEncontrada = await BuscarTarefaPorId(idTarefa);
			Tarefas tarefaAtualizada = dados.ToTarefas(tarefaEncontrada);
			context.Tarefas.Update(tarefaAtualizada);
			await context.SaveChangesAsync();
			return tarefaAtualizada;
		}

		public async Task<Tarefas> AtualizarStatusTarefa(Guid idTarefa, bool status)
		{
			Tarefas tarefa = await BuscarTarefaPorId(idTarefa);
			tarefa.Concluido = status;
			context.Tarefas.Update(tarefa);
			await context.SaveChangesAsync();
			return tarefa;
		}


		public async Task ExcluirTarefa(Guid idTarefa, Guid idUsuario)
		{
			Tarefas tarefa = await context.Tarefas.Include(p => p.Itens).FirstOrDefaultAsync
				(p => p.Id == idTarefa) ?? throw new TarefaNaoEncontradaException($"Tarefa com id {idTarefa} não foi encontrada");
			context.RemoveRange(tarefa);
			await AtualizarQtdTarefas(idUsuario, false);
			await context.SaveChangesAsync();
			return;
		}
	}
}
