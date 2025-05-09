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
				Item = dados.item,
				Concluido = false
			};
		}

		public static CheckItem ToCheckItem(this CheckItemUpdate dados, CheckItem item)
		{
			item.Item = dados.item ?? item.Item;
			return item;
		}
	}
}
