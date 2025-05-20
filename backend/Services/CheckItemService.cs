using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Resultados;
using backend.Services.Interface;

namespace backend.Services
{
	public class CheckItemService(ICheckItemRepositorio itemRepositorio) : ICheckItemService
	{
		public async Task<Result<List<CheckItemResponse>, Error>> AdicionarItensNaLista(Guid idTarefa, List<CheckItemCreate> itens)
		{
			if (itens.Count == 0) return CheckItemFailure.ItensVazios;
			List<CheckItem> listaItens = itens.Select(p => p.ToCheckItem(idTarefa)).ToList();
			List<CheckItem>? resposta = await itemRepositorio.AdicionarItensNaLista(idTarefa, listaItens);
			if (resposta is null) return TarefasFailure.TarefaNaoEncontrada;
			return resposta.Select(p => p.ToCheckItemResponse()).ToList();
		}

		public async Task<Result<List<CheckItemResponse>, Error>> EditarItens(Guid idTarefa, List<CheckItemUpdate> itens)
		{
			if (itens.Count == 0) return CheckItemFailure.ItensVazios;
			List<CheckItem>? listaItens = await itemRepositorio.BuscarItensPorId(
					itens.Select(p => p.IdItem).ToList());

			if (listaItens is null) return CheckItemFailure.ItemNaoEncontrado;
			List<CheckItem> listaAtualizada = itens.Select((item, index) =>
				item.ToCheckItem(listaItens[index])
			).ToList();
			List<CheckItem>? resposta = await itemRepositorio.EditarItens(idTarefa, listaAtualizada);
			if (resposta is null) return TarefasFailure.TarefaNaoEncontrada;
			return resposta.Select(p => p.ToCheckItemResponse()).ToList();
		}
	}
}
