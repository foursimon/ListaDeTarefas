using backend.Models.Dtos.UsuarioDto;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos.TarefasDto
{
	public class TarefasCreate
	{
		[Required(ErrorMessage = "Insira o título da tarefa")]
		public required string Titulo { get; set; }
		public DateOnly? DataDeEncerramento { get; set; }
		public string? Descricao { get; set; }
		[Required(ErrorMessage = "Insira o tipo de tarefa")]
		public required string Tipo { get; set; }
	}
}
