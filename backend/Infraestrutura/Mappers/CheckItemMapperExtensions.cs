using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Infraestrutura.Mappers
{
	public static class CheckItemMapperExtensions
	{
		public static CheckItemResponse ToCheckItemResponse(this CheckItem item)
		{
			return new CheckItemResponse(
				item.Id,
				item.Item,
				item.Concluido
			);
		}
		public static CheckItem ToCheckItem(this CheckItemCreate dados)
		{
			return new CheckItem
			{
				Item = dados.Item,
				Concluido = false
			};
		}
		public static CheckItem ToCheckItem(this CheckItemCreate dados, Guid idTarefa)
		{
			return new CheckItem
			{
				Item = dados.Item,
				Concluido = false,
				IdTarefa = idTarefa
			};
		}

		public static CheckItem ToCheckItem(this CheckItemUpdate dados, CheckItem item)
		{
			item.Item = dados.Item ?? item.Item;
			item.Concluido = dados.Concluido ?? item.Concluido;
			return item;
		}
	}
}
