using backend.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos.UsuarioDto
{
	public class UsuarioCreate
	{
		[Required(ErrorMessage = "Insira seu nome")]
		public required string Nome { get; set; }
		[Required(ErrorMessage = "Insira seu e-mail")]
		public required string Email { get; set; }
		[Required(ErrorMessage = "Insira sua senha")]
		public required string Senha { get; set; }
	}
}
