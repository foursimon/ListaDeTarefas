using backend.BancoDeDados;
using backend.Repositorios;
using backend.Repositorios.Interface;
using backend.Security;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Scalar.AspNetCore;

namespace backend.Infraestrutura
{
	public static class DependeciesConfig
	{
		public static void AddDependecies(this WebApplicationBuilder builder)
		{
			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.AddDbContext<TarefasDbContext>(opt =>
				opt.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection")!));
			AddRepositorios(builder);
		}

		public static void AddRepositorios(WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
			builder.Services.AddScoped<ISenhaHasher, SenhaHasher>();
		}
		public static void AddApiDocumentation(this WebApplication app)
		{
			app.MapOpenApi();
			app.MapScalarApiReference("scalar/v1", opt =>
			{
				opt.Title = "Tarefas API";
				opt.Theme = ScalarTheme.BluePlanet;
				opt.WithDefaultHttpClient(ScalarTarget.Node, ScalarClient.Fetch);
			});
		}
	}
}
