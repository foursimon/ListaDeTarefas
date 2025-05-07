using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Models.Entities;
using backend.Models.Tokens;
using System.Security.Cryptography;

namespace backend.Security
{
	public class TokenGerador (IConfiguration configuracao) : ITokenGerador
	{
		private const string algoritmo = SecurityAlgorithms.HmacSha512;
		public string CriarToken(string nome, Guid idUsuario, string email)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, nome),
				new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
				new Claim(ClaimTypes.Email, email)
			};
			var chave = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(configuracao.GetValue<string>("JWT:Token")!));
			//Por ter escolhido o algoritmo de segurança HmacSha512, é necessário
			//que a chave tenha tamanho de 64 bytes (512 bit)
			var credenciais = new SigningCredentials(chave, algoritmo);

			var token = new JwtSecurityToken(
				issuer: configuracao.GetValue<string>("JWT:Issuer"),
				audience: configuracao.GetValue<string>("JWT:Audience"),
				claims,
				expires: DateTime.UtcNow.AddMinutes(20),
				signingCredentials: credenciais
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public TokenCriado CriarTokenRecarga()
		{
			var numeroAleatorio = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(numeroAleatorio);
			TokenCriado tokenRecarga = new TokenCriado()
			{
				TokenRecarga = Convert.ToBase64String(numeroAleatorio),
				TempoToken = DateTime.UtcNow.AddDays(7)
			};
			return tokenRecarga;
		}
	}
}
