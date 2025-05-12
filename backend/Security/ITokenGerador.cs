using backend.Models.Entities;
using backend.Models.Dtos;

namespace backend.Security
{
	public interface ITokenGerador
	{
		public string CriarToken(string nome, Guid idUsuario, string email);
		public TokenCriado CriarTokenRecarga(Guid idUsuario);
		public TokenCriado RecarregarToken(Usuario usuario, string tokenRecebido);

		public Guid PegarIdUsuarioToken(string token);

	}
}
