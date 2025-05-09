using System.Text.Json.Serialization;

namespace backend.Models.Entities
{
	public class CheckItem
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string Item { get; set; }
		public bool Concluido { get; set; } = false;
		public Guid IdTarefa { get; set; }

		[JsonIgnore]
		public Tarefas? Tarefa { get; set; }
	}
}
