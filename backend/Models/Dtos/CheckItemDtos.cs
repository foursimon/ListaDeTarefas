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
		string Item
	);

	public sealed record CheckItemUpdate(
		[Required(ErrorMessage = "É necessário o id do item a ser atualizado")]
		Guid IdItem,
		string? Item,
		bool? Concluido
	);
};
