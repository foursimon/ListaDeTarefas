using backend.Models.Entities;
using backend.Models.Dtos;

namespace backend.Security.Interface
{
	public interface ITokenGerador
	{
		public string CriarToken(string nome, Guid idUsuario, string email);
		public TokenCriado CriarTokenRecarga(Guid idUsuario);
		public TokenCriado? RecarregarToken(Usuario usuario, string tokenRecebido);

		public string? PegarIdUsuarioToken(string token);

	}
}
