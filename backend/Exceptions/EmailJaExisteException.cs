namespace backend.Exceptions
{
	public class EmailJaExisteException : Exception
	{
		public EmailJaExisteException()
		{
		}

		public EmailJaExisteException(string? message) : base(message)
		{
		}
	}
}
