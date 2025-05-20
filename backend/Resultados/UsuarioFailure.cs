namespace backend.Resultados
{
	public static class UsuarioFailure
	{
		public static Error UsuarioNaoEncontrado => Error.NotFound
			("Usuário não encontrado", "A conta do usuário não foi encontrada");
		public static Error LoginErrado => Error.Unauthorized
			("E-mail ou senha incorretos", "O e-mail ou a senha informados estão incorretos");

		public static Error EmailJaExiste => Error.Conflict
			("E-mail já cadastrado", "o e-mail informado já está vinculado a uma conta");

		public static Error TokenInvalido => Error.Unauthorized
			("O Token é inválido", "O token enviado é inválido");

	}
}
