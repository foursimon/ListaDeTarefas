namespace backend.Exceptions.UsuarioException
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
