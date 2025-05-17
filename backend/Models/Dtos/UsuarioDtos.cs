using System.ComponentModel.DataAnnotations;

namespace backend.Models.Dtos
{
	public sealed record UsuarioResponse(
		string Nome,
		string Email,
		int QuantidadeTarefa
	);

	public sealed record UsuarioCreate(
		[Required(ErrorMessage = "Insira seu nome")]
		string Nome,
		[Required(ErrorMessage = "Insira seu e-mail")]
		[EmailAddress(ErrorMessage = "E-mail inválido")]
		string Email,
		[Required(ErrorMessage = "Insira sua senha")]
		string Senha
	);

	public sealed record UsuarioLogin(
		[Required(ErrorMessage = "Insira seu email")]
		string Email,
		[Required(ErrorMessage = "Insira sua senha")]
		string Senha
	);

	public sealed record UsuarioUpdate(
		string? Nome,
		string? Email,
		string? Senha
	);
}
