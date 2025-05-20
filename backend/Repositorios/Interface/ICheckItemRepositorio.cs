using backend.Models.Entities;

namespace backend.Repositorios.Interface
{
	public interface ICheckItemRepositorio
	{
		public Task<List<CheckItem>?> AdicionarItensNaLista(Guid idTarefa, List<CheckItem> itens);
		public Task<List<CheckItem>?> BuscarItensPorId(List<Guid> idItens);
		public Task<List<CheckItem>?> EditarItens(Guid idTarefa, List<CheckItem> itens);
	}
}
