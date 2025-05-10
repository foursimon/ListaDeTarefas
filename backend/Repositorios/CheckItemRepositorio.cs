using backend.BancoDeDados;
using backend.Exceptions.CheckItemException;
using backend.Exceptions.TarefasException;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class CheckItemRepositorio(TarefasDbContext context) : ICheckItemRepositorio
	{
		public async Task<List<CheckItem>> AdicionarItensNaLista(Guid idTarefa, List<CheckItem> itens)
		{
			await VerificarTarefa(idTarefa);
			context.CheckItems.AddRange(itens);
			await context.SaveChangesAsync();
			return itens;
		}

		private async Task VerificarTarefa(Guid idTarefa)
		{
			Tarefas tarefa = await context.Tarefas.FindAsync(idTarefa) ??
				throw new TarefaNaoEncontradaException($"Tarefa com id {idTarefa} não foi encontrada");
		}
		public async Task<List<CheckItem>> BuscarItensPorId(List<Guid> idItens)
		{
			List<CheckItem> itens = await context.CheckItems.Where(p => idItens.Contains(p.Id)).ToListAsync();
			if(itens.Count != idItens.Count)
				throw new ItemNaoExisteException("Um ou mais itens recebidos não existem");
			return itens;
		}
		public async Task<List<CheckItem>> EditarItens(Guid idTarefa, List<CheckItem> itens)
		{
			await VerificarTarefa(idTarefa);
			context.CheckItems.UpdateRange(itens);
			await context.SaveChangesAsync();
			return itens;
		}
	}
}
