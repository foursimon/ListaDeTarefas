using backend.Resultados;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace backend.Infraestrutura
{
	public static class ProblemExtensions
	{
		public static ProblemDetails ToProblemDetails(this Error erro)
		{
			return new ProblemDetails
			{
				Type = $"https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/{erro.CodigoStatus}",
				Title = erro.Titulo,
				Detail = erro.Descricao,
				Status = erro.CodigoStatus
			};
		}
	}
}
