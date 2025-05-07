namespace backend.Exceptions
{
	public class TokenInvalidoException : Exception
	{
		public TokenInvalidoException()
		{
		}

		public TokenInvalidoException(string? message) 
			: base(message) { }
	}
}
