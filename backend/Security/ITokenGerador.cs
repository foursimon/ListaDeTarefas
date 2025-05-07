using backend.Models.Entities;
using backend.Models.Tokens;

namespace backend.Security
{
	public interface ITokenGerador
	{
		public string CriarToken(string nome, Guid idUsuario, string email);
		public TokenCriado CriarTokenRecarga();

	}
}
