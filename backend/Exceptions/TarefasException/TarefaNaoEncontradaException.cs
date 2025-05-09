namespace backend.Exceptions.TarefasException
{
	public class TarefaNaoEncontradaException : KeyNotFoundException
	{
		public TarefaNaoEncontradaException()
		{
		}

		public TarefaNaoEncontradaException(string? message) : base(message)
		{
		}
	}
}
