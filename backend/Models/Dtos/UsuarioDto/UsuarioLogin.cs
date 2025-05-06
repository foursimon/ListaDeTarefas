using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos.UsuarioDto
{
	public class UsuarioLogin
	{
		[Required(ErrorMessage = "insira seu email")]
		public required string Email { get; set; }
		[Required(ErrorMessage = "insira sua senha")]
		public required string Senha { get; set; }
	}
}
