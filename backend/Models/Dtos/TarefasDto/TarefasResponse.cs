using backend.Models.Dtos.CheckItemDto;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;

namespace backend.Models.Dtos.TarefasDto
{
	public class TarefasResponse
	{
		public Guid Id { get; set; }
		public required string Titulo { get; set; }
		public bool Concluido { get; set; } = false;
		public DateOnly DataDeEncerramento { get; set; }
		public string? Descricao { get; set; }
		public required string Tipo { get; set; }
		public UsuarioResponse? Usuario { get; set; }
		public List<CheckItemResponse>? Itens { get; set; }
	}
}
