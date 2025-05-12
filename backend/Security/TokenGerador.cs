using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Models.Entities;
using backend.Models.Dtos;
using System.Security.Cryptography;
using backend.Exceptions.UsuarioException;

namespace backend.Security
{
	public class TokenGerador (IConfiguration configuracao) : ITokenGerador
	{
		private const string algoritmo = SecurityAlgorithms.HmacSha512;
		private const string algoritmoRecarga = SecurityAlgorithms.HmacSha256;
		public string CriarToken(string nome, Guid idUsuario, string email)
		{
			//Criando uma identidade para o usuário que entrar em sua conta.
			var claims = new List<Claim>
			{
				//Name: o nome do usuário
				new Claim(ClaimTypes.Name, nome),
				//NameIdentifier: o identificador da assinatura do usuário
				new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
				//Email: o email do usuário
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
				expires: DateTime.UtcNow.AddMinutes(10),
				signingCredentials: credenciais
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		//Método responsável para criar o token de recarga.
		public TokenCriado CriarTokenRecarga(Guid idUsuario)
		{
			//Um token de recarga tem a função de gerar um novo
			//token de acesso para o usuário não precisar entrar na sua conta
			//toda hora.
			//Um token de acesso deve ter tempo de vida útil bem curto,
			//pois, mesmo que algum invasor pegue o token de acesso,
			//ele não terá muito tempo para causar muito dano no sistema.
			//Um token de recarga, portanto, irá gerar um novo token de acesso
			//para o usuário quando o tempo de vida útil do token de acesso
			//acabar.
			//Preciso que o token de recarga armazene o id do usuário para que eu possa
			//verificar se o id do usuário existe no banco de dados e retornar um novo
			//token de recarga e um token de acesso
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
				new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
			};
			var chave = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(configuracao.GetValue<string>("JWT:TokenRecarga")!));
			var credenciais = new SigningCredentials(chave, algoritmoRecarga);

			var token = new JwtSecurityToken(
				issuer: configuracao.GetValue<string>("JWT:Issuer"),
				audience: configuracao.GetValue<string>("JWT:Audience"),
				claims,
				expires: DateTime.UtcNow.AddDays(7),
				signingCredentials: credenciais
			);
			//Atribuindo o token de recarga para classe TokenCriado.
			//O tempo de vida do token de recarga também é atribuído.
			//Por padrão, coloco sete dias para o tempo de vida.
			//Quando esse tempo passar, o usuário terá que entrar em sua conta
			//novamente.
			var tokenRecarga = new JwtSecurityTokenHandler().WriteToken(token);
			var tempoToken = DateTime.UtcNow.AddDays(7);
			return new TokenCriado(null, tokenRecarga, tempoToken);
		}

		public Guid PegarIdUsuarioToken(string tokenRecebido)
		{
			var handler = new JwtSecurityTokenHandler();
			if (!handler.CanReadToken(tokenRecebido))
				throw new Exception("Não é possível ler o token enviado");
			var token = handler.ReadJwtToken(tokenRecebido);
			var idUsuario = token.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
			if (idUsuario is null) throw new Exception("Usuário não foi encontrado no token");
			return Guid.Parse(idUsuario);

		}

		public TokenCriado RecarregarToken(Usuario usuario, string tokenRecebido)
		{
			if(tokenRecebido != usuario.TokenRecarga || tokenRecebido is null
				|| usuario.TempoToken <= DateTime.UtcNow)
			{
				throw new TokenInvalidoException("O token de recarga enviado é inválido.");
			}

			var tokenAcesso = CriarToken(usuario.Nome, usuario.Id, usuario.Email);
			var tokenRecarga = CriarTokenRecarga(usuario.Id).TokenRecarga;
			var tempoToken = DateTime.UtcNow.AddDays(7);
			return new TokenCriado(tokenAcesso, tokenRecarga, tempoToken);
		}
	}
}
