namespace backend.Models.Dtos
{
	public sealed record CheckItemResponse(
		Guid Id,
		string Item,
		bool Concluido
	);
}
