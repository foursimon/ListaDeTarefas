namespace backend.Models.Security
{
	public interface ISenhaHasher
	{
		public string CriptografarSenha(string senha);

		public bool VerificarSenha(string senha, string senhaCriptografada);
	}
}
