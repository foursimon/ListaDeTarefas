using System.Text.Json.Serialization;

namespace backend.Models.Tokens
{
	public class TokenCriado
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? TokenAcesso { get; set; } = null;
		public required string TokenRecarga {  get; set; }
		public DateTime? TempoToken { get; set; } = DateTime.UtcNow.AddDays(7);
	}
}
