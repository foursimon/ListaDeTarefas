using backend.Models.Entities;

namespace backend.Models.Dtos.UsuarioDto
{
	public class UsuarioResponse
	{
		public Guid Id { get; set; }
		public required string Nome { get; set; }
		public required string Email { get; set; }
		public required string Senha { get; set; }
		public int QuantidadeTarefa { get; set; }
	}
}
