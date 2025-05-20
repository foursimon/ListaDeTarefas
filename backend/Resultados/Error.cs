using Mysqlx.Expr;

namespace backend.Resultados
{
	public class Error
	{
		public string Titulo { get; }
		public string Descricao { get; }

		public int CodigoStatus { get; }

		private Error(string titulo, string descricao, int codigoStatus)
		{
			Titulo = titulo;
			Descricao = descricao;
			CodigoStatus = codigoStatus;
		}
		public static Error NotFound(string titulo, string descricao) =>
			new Error(titulo, descricao, 404);
		public static Error BadRequest(string titulo, string descricao) =>
			new Error(titulo, descricao, 400);
		public static Error Unauthorized(string titulo, string descricao) =>
			new Error(titulo, descricao, 401);
		public static Error Conflict(string titulo, string descricao) =>
			new Error(titulo, descricao, 400);
	}
}
