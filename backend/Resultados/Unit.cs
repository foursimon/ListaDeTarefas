namespace backend.Resultados
{

	//A struct Unit serve apenas para retornar um valor vazio.
	//O nome "Unit" é utilizado apenas pela padronização.
	//Seu uso vem do fato da classe Result ter que ser de dois tipos,
	//como o valor void ou null não é aceito como um tipo, a struct Unit é criada
	//para cumprir este papel descrito.
	public readonly struct Unit
	{
		public static readonly Unit Value = new Unit();
	}
}
