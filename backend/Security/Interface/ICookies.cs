using backend.Models.Dtos;

namespace backend.Security.Interface
{
	public interface ICookies
	{
		public void ColocarTokensNoCookie(TokenResponse tokens, HttpContext httpContext);
		public void RemoverCookies(HttpContext httpContext);
	}
}
