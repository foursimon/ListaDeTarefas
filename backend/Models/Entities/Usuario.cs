﻿
namespace backend.Models.Entities
{
	public sealed class Usuario
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string Nome { get; set; }
		public required string Email { get; set; }
		public required string Senha { get; set; }
		public int QuantidadeTarefa { get; set; } = 0;
		public DateTime? TempoToken {get; set; }
		public string? TokenRecarga{ get; set; }
		public List<Tarefas>? Tarefa {  get; set; }
	}
}
