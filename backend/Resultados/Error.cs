using Mysqlx.Expr;

namespace backend.Resultados
{
	//A classe Error éfeita para padronizar o retorno de possíveis
	//erros esperados.
	public class Error
	{
		//Os atributos abaixo condizem com o seguintes atributos de ProblemDetails:
		//Título para Title;
		//Descrição para Detail;
		//CodigoStatus para Status;
		public string Titulo { get; }
		public string Descricao { get; }

		public int CodigoStatus { get; }

		private Error(string titulo, string descricao, int codigoStatus)
		{
			Titulo = titulo;
			Descricao = descricao;
			CodigoStatus = codigoStatus;
		}

		//O construtor é privado pois Error só é instanciado através dos métodos
		//abaixo.
		//Dessa forma, consigo determinar cada tipo de retorno de erro e deixar
		//mais claro seu tipo através do nome dos métodos.
		public static Error NotFound(string titulo, string descricao) =>
			new Error(titulo, descricao, 404);
		public static Error BadRequest(string titulo, string descricao) =>
			new Error(titulo, descricao, 400);
		public static Error Unauthorized(string titulo, string descricao) =>
			new Error(titulo, descricao, 401);
		public static Error Conflict(string titulo, string descricao) =>
			new Error(titulo, descricao, 409);
	}
}
