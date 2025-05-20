using backend.BancoDeDados;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositorios
{
	public class CheckItemRepositorio(TarefasDbContext context) : ICheckItemRepositorio
	{
		public async Task<List<CheckItem>?> AdicionarItensNaLista(Guid idTarefa, List<CheckItem> itens)
		{
			if (!await VerificarSeTarefaExiste(idTarefa)) return null;
			context.CheckItems.AddRange(itens);
			await context.SaveChangesAsync();
			return itens;
		}

		private async Task<bool> VerificarSeTarefaExiste(Guid idTarefa)
		{
			var tarefa = await context.Tarefas.FindAsync(idTarefa);
			return tarefa is not null;
		}
		public async Task<List<CheckItem>?> BuscarItensPorId(List<Guid> idItens)
		{
			List<CheckItem> itens = await context.CheckItems.Where(p => idItens.Contains(p.Id)).ToListAsync();
			if (itens.Count != idItens.Count) return null;
			return itens;
		}
		public async Task<List<CheckItem>?> EditarItens(Guid idTarefa, List<CheckItem> itens)
		{
			if (!await VerificarSeTarefaExiste(idTarefa)) return null;
			context.CheckItems.UpdateRange(itens);
			await context.SaveChangesAsync();
			return itens;
		}
	}
}
