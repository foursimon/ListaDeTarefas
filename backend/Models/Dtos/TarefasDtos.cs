using System.ComponentModel.DataAnnotations;
namespace backend.Models.Dtos
{
	public sealed record TarefasResponse(
		Guid Id,
		string Titulo,
		bool Concluido,
		DateOnly DataDeEncerramento,
		string? Descricao,
		string Tipo, 
		List<CheckItemResponse>? Itens 
	);

	public sealed record TarefasCreate(
		[Required(ErrorMessage = "Insira o título da tarefa")]
		string Titulo,
		DateOnly? DataDeEncerramento, 
		string? Descricao, 
		[Required(ErrorMessage = "Insira o tipo de tarefa")]
		string Tipo
	);

	public sealed record TarefasUpdate(
		string? Titulo,
		DateOnly? DataDeEncerramento,
		string? Descricao,
		string? Tipo
	);
}
