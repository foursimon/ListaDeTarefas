using backend.Models.Dtos;
using backend.Models.Entities;

namespace backend.Infraestrutura.Mappers
{
	public static class UsuarioMapperExtensions
	{
		public static UsuarioResponse ToUsuarioResponse(this Usuario usuario)
		{
			return new UsuarioResponse(
				usuario.Id,
				usuario.Nome,
				usuario.Email,
				usuario.QuantidadeTarefa
			);
		}

		public static Usuario ToUsuario(this UsuarioCreate dados)
		{
			return new Usuario
			{
				Nome = dados.Nome,
				Email = dados.Email,
				Senha = dados.Senha,
			};
		}

		public static Usuario ToUsuario(this UsuarioUpdate dados, Usuario usuario)
		{
			usuario.Nome = dados.Nome ?? usuario.Nome ;
			usuario.Email = dados.Email ?? usuario.Email;
			usuario.Senha = dados.Senha ?? usuario.Senha;
			return usuario;
		}
	}
}
