namespace backend.Resultados
{
	public static class TarefasFailure
	{
		public static Error QuantidadeExcedida => Error.BadRequest
			("Quantidade máxima de tarefas excedida", 
			"Não foi possível criar mais uma tarefa por ter exceido quantidade máxima");
		public static Error TarefaNaoEncontrada => Error.NotFound
			("Tarefa não foi encontrada", "A tarefa com ID informado não foi encontrada");
	}
}
