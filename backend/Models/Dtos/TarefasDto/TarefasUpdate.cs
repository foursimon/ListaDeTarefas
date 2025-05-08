using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos.TarefasDto
{
	public class TarefasUpdate
	{
		public string? Titulo { get; set; }
		public DateOnly? DataDeEncerramento { get; set; }
		public string? Descricao { get; set; }
		public string? Tipo { get; set; }
	}
}
