namespace backend.Exceptions.CheckItemException
{
	public class ItemNaoExisteException : ArgumentException
	{
		public ItemNaoExisteException(){}

		public ItemNaoExisteException(string? mensagem) 
			: base(mensagem){}
	}
}
