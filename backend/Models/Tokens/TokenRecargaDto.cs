using System.ComponentModel.DataAnnotations;

namespace backend.Models.Tokens
{
	public class TokenRecargaDto
	{
		[Required(ErrorMessage = "É necessário o Id de usuário")]
		public required Guid IdUsuario { get; set; }

		[Required(ErrorMessage = "É necessário o Token de recarga")]
		public required string TokenRecarga { get; set; }
	}
}
