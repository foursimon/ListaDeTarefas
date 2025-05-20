using backend.Models.Dtos;
using backend.Resultados;
using backend.Models.Entities;

namespace backend.Services.Interface
{
	public interface ICheckItemService
	{
		public Task<Result<List<CheckItemResponse>, Error>> AdicionarItensNaLista(Guid idTarefa, List<CheckItemCreate> itens);
		public Task<Result<List<CheckItemResponse>, Error>> EditarItens(Guid idTarefa, List<CheckItemUpdate> itens);

	}
}
