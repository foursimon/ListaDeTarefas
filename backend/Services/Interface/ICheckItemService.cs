using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Services.Interface
{
	public interface ICheckItemService
	{
		public Task<List<CheckItemResponse>> AdicionarItensNaLista(Guid idTarefa, List<CheckItemCreate> itens);
		public Task<List<CheckItemResponse>> EditarItens(Guid idTarefa, List<CheckItemUpdate> itens);

	}
}
