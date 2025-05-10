namespace backend.Exceptions.CheckItemException
{
	public class ItensVaziosException : ArgumentException
	{
		public ItensVaziosException(){}

		public ItensVaziosException(string? mensagem) 
			: base(mensagem){}
	}
}
