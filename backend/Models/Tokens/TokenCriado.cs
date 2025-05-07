namespace backend.Models.Tokens
{
	public class TokenCriado
	{
		public required string TokenRecarga {  get; set; }
		public DateTime? TempoToken { get; set; } = DateTime.UtcNow.AddDays(7);
	}
}
