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
		string Tipo,
		[Required(ErrorMessage = "Adicione uma lista de itens")]
		[Length(minimumLength:3, maximumLength: 20, ErrorMessage = "A lista de tarefas deve ter 3 itens e não pode exceder 20 itens")]
		List<CheckItemCreate> Itens
	);

	public sealed record TarefasUpdate(
		string? Titulo,
		DateOnly? DataDeEncerramento,
		string? Descricao,
		string? Tipo
	);
}
