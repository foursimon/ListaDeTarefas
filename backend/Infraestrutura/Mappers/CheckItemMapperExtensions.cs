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
	}
}
