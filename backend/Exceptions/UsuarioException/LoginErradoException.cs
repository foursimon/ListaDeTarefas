namespace backend.Exceptions.UsuarioException
{
	public class LoginErradoException : Exception
	{
		public LoginErradoException()
		{
		}

		public LoginErradoException(string? mensagem) : base(mensagem)
		{
		}

		public LoginErradoException
			(string? mensagem, Exception? innerException)
			: base(mensagem, innerException) { }
	}
}
