namespace backend.Resultados
{
	public static class CheckItemFailure
	{
		public static Error ItemNaoEncontrado => Error.NotFound
			("Item não foi encontrado", "Um ou mais itens recebidos não foram encontrados");

		public static Error ItensVazios => Error.BadRequest
			("Não é possível adicionar itens vazio na lista", 
			"Itens vazios não podem ser adicionados na lista");
	}
}
