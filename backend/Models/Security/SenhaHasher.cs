using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
namespace backend.Models.Security
{
	public class SenhaHasher : ISenhaHasher
	{
		//O número recomendado de iterações de hash é 100000
		private const int iteracoes = 100000;
		//O tamanho mínimo recomendado do salt é de 16 bytes
		private const int tamanhoSalt = 16;
		//O tamanho mínimo recomendado do hash é de 32 bytes
		private const int tamanhoHash = 32;
		//Definindo o algoritmo de criptografia de hash
		private static readonly HashAlgorithmName algoritmo = HashAlgorithmName.SHA512;
		
		//Método responsável por criptografar senhas.
		public string CriptografarSenha(string senha)
		{
			//O salt irá ser uma chave da senha criptografada.
			//Ele serve para impedir que os valores criptografados gerados
			//sejam repetidos e reforça ainda mais a segurança, pois
			//impede ataques de descriptografia com força bruta, o que o hash
			//sozinho é incapaz de fazer.
			//Gerando um salt aleatório com o tamanho definido na variável
			//tamanhoSalt.
			byte[] salt = RandomNumberGenerator.GetBytes(tamanhoSalt);
			//Criptografando a senha;
			byte[] hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, 
				iteracoes, algoritmo, tamanhoHash);
			//Divido a senha criptografada em duas partes: o hash primeiro e o salt segundo.
			//Faço isso para armazenar os dois no campo senha do banco de dados e para
			//identificar qual é o salt e qual é o hash
			return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";

		}

		//Método responsável por verificar se a senha recebida equivale a senha
		//armazenada no banco de dados.
		public bool VerificarSenha(string senha, string senhaCriptografada)
		{
			//Dividindo a senha criptografada para separar o hash e o salt.
			string[] partes = senhaCriptografada.Split('-');
			//Armazenando o hash da senha criptografada.
			byte[] hash = Convert.FromHexString(partes[0]);
			//Armazenando o salt da senha criptografada.
			byte[] salt = Convert.FromHexString(partes[1]);
			//Criptografando a senha recebida com o mesmo salt para gerar
			//o mesmo valor criptografado.
			byte[] hashRecebido = Rfc2898DeriveBytes.Pbkdf2(senha, salt, iteracoes, algoritmo, tamanhoHash);
			//Comparando a senha recebida, que agora também foi criptografada com o mesmo
			//valor de salt, com a senha criptografada salva no banco de dados.
			return CryptographicOperations.FixedTimeEquals(hash, hashRecebido);
		}
	}
}
