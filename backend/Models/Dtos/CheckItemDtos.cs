using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos
{
	public sealed record CheckItemResponse(
		Guid Id,
		string Item,
		bool Concluido
	);

	public sealed record CheckItemCreate(
		[Required(ErrorMessage = "Escreva o item da lista")]
		string item
	);

	public sealed record CheckItemUpdate(
		string? item	
	);
};
