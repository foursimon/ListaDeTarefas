namespace backend.Resultados
{
	//Esta classe segue um padrão resultado (Result pattern) onde um objeto
	//de sucesso ou erro é retornado ao invés de jogar exceções sempre que um
	//erro esperado acontecer.
	//Jogar exceções consomem tempo e memória, e podem acabar reduzindo a eficiência
	//do sistema.
	//A classe Result, neste caso, irá ter dois tipos de valores.
	//O primeiro depende da tarefa e o segundo sempre deverá ser do tipo
	//da classe Error que criei.
	public class Result<TValue, TError>
	{
		//IsSucces é um atributo usado para verificar se uma determinada tarefa
		//teve êxito ou não.
		public bool IsSuccess { get; }
		//TValue representa o valor recebido quando uma função teve êxito em sua 
		//operação. Ele pode ser nulo pois não é garantido que uma operação
		//terá sucesso ou não.
		public TValue? Value { get; }
		//TError representa o valor de uma instância da classe Error, servindo
		//para mostrar qual tipo de erro aconteceu.
		//Ele pode ser nulo pois nem sempre uma tarefa irá dar erro.
		public TError? Error { get; }

		//Temos dois construtores privados nesta classe, pois criei operadores
		//de conversão implícita para instanciarem a classe.
		private Result(TValue? value)
		{
			IsSuccess = true;
			Value = value;
			Error = default;
		}
		private Result(TError? error)
		{
			IsSuccess = false;
			Value = default;
			Error = error;
		}
		//O operador implícito serve para converter um determinado valor para outro
		//valor de forma implícita. Dessa forma, não é necessário especificar
		//em return que o valor a ser retornado será um Result do tipo TValue e
		//do tipo TError.
		//O primeiro operador implícito irá converter o TValue recebido
		//para uma instância da classe Result usando o primeiro construtor privado.
		//Ele serve para retornar um valor de sucesso.
		public static implicit operator Result<TValue, TError>(TValue value) => 
			new Result<TValue, TError>(value);
		//O segundo operador implícito irá converter o TError recebido para uma
		//instância da classe Result.
		//Ele serve para retornar um Erro recebido
		public static implicit operator Result<TValue, TError>(TError error) => 
			new Result<TValue, TError>(error);
	}
}
