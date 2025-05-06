namespace backend.Models.Entities
{
	public sealed class Tarefas
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string Titulo { get; set; }
		public bool Concluido { get; set; } = false;
		public DateOnly DataDeEncerramento { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
		public string? Descricao { get; set; }
		public required string Tipo { get; set; }
		public required Guid IdUsuario { get; set; }
		public Usuario? Usuario { get; set; }
		public List<CheckItem>? Itens { get; set; }


	}
}
