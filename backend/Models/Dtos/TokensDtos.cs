using System.Text.Json.Serialization;

namespace backend.Models.Dtos
{
	public sealed record TokenCriado(
		string? TokenAcesso,
		string TokenRecarga,
		DateTime? TempoToken
	);

	public sealed record TokenResponse(
		string TokenAcesso,
		string TokenRecarga
	);
}
