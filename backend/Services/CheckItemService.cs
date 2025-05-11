using backend.Exceptions.CheckItemException;
using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Services.Interface;

namespace backend.Services
{
	public class CheckItemService(ICheckItemRepositorio itemRepositorio) : ICheckItemService
	{
		public async Task<List<CheckItemResponse>> AdicionarItensNaLista(Guid idTarefa, List<CheckItemCreate> itens)
		{
			if (itens.Count == 0)
				throw new ItensVaziosException("Itens novos não podem estar vazios");
			List<CheckItem> listaItens = itens.Select(p => p.ToCheckItem(idTarefa)).ToList();
			List<CheckItem> resposta = await itemRepositorio.AdicionarItensNaLista(idTarefa, listaItens);
			return resposta.Select(p => p.ToCheckItemResponse()).ToList();
		}

		public async Task<List<CheckItemResponse>> EditarItens(Guid idTarefa, List<CheckItemUpdate> itens)
		{
			if (itens.Count == 0)
				throw new ItensVaziosException("Itens novos não podem estar vazios");
			List<CheckItem> listaItens = await itemRepositorio.BuscarItensPorId(
					itens.Select(p => p.IdItem).ToList());
			List<CheckItem> listaAtualizada = new List<CheckItem>();
			for(int index = 0; index < listaItens.Count; index++)
			{
				CheckItem item = itens[index].ToCheckItem(listaItens[index]);
				listaAtualizada.Add(item);
			} 
			List<CheckItem> resposta = await itemRepositorio.EditarItens(idTarefa, listaAtualizada);
			return resposta.Select(p => p.ToCheckItemResponse()).ToList();
		}
	}
}
