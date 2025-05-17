using backend.Models.Dtos;
using backend.Security.Interface;

namespace backend.Security
{
	public class Cookies : ICookies
	{
		//Método responsável por armazenar informações em Cookies
		public void ColocarTokensNoCookie(TokenResponse tokens, HttpContext httpContext)
		{
			//Aqui estou armazenando o token de acesso no cookie
			httpContext.Response.Cookies.Append("TOKEN_ACESSO", tokens.TokenAcesso,
				new CookieOptions
				{
					//Expires indica o tempo que o cookie irá expirar,
					Expires = DateTime.UtcNow.AddMinutes(10),
					//HttpOnly indica se o valor no cookie não pode ser acessado
					//por scripts no lado cliente (frontend)
					HttpOnly = true,
					//IsEssential indica que o valor que está sendo armazenado no cookie
					//é essencial para o funcionamento correto do sistema
					IsEssential = true,
					//Secure indica se o cookie só pode ser transmitido em conexão
					//HTTPS.
					Secure = true,
					//SameSite indica se o cookie pode ser usado entre sites
					SameSite = SameSiteMode.Strict
				});
			httpContext.Response.Cookies.Append("TOKEN_RECARGA", tokens.TokenRecarga,
				new CookieOptions
				{
					Expires = DateTime.UtcNow.AddDays(7),
					HttpOnly = true,
					IsEssential = true,
					Secure = true,
					SameSite = SameSiteMode.Strict
				}
			);

		}
		//Método responsável por remover os cookies com os tokens de autenticação
		//e recarga
		public void RemoverCookies(HttpContext httpContext)
		{
			//Usando o método "Delete", posso remover os cookies salvos no contexto
			//da conexão atual.
			httpContext.Response.Cookies.Delete("TOKEN_ACESSO");
			httpContext.Response.Cookies.Delete("TOKEN_RECARGA");
		}
	}
}
