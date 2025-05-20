using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backend.Infraestrutura
{
	public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
			Exception exception, CancellationToken cancellationToken)
		{
			_logger.LogError(exception, "Uma Exceção ocorreu: {Message}", exception.Message);

			var detalhesDoProblema = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Reference/Status/500",
				Title = "Algo inesperado ocorreu."
			};

			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await httpContext.Response.WriteAsJsonAsync(detalhesDoProblema, cancellationToken);

			return true;
		}
	}
}
