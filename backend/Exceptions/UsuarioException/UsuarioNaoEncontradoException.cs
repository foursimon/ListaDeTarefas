namespace backend.Exceptions.UsuarioException
{
	public class UsuarioNaoEncontradoException : KeyNotFoundException
	{
		public UsuarioNaoEncontradoException() { }
		public UsuarioNaoEncontradoException(string mensagem)
			: base(mensagem) { }
		public UsuarioNaoEncontradoException(string mensagem, Exception excecao)
		: base(mensagem, excecao)
		{
		}
	}
}
