using backend.BancoDeDados;
using backend.Repositorios;
using backend.Repositorios.Interface;
using backend.Security;
using backend.Services;
using backend.Services.Interface;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Scalar.AspNetCore;

namespace backend.Infraestrutura
{
	public static class DependeciesConfig
	{
		public static void AddDependecies(this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<TarefasDbContext>(opt =>
				opt.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection")!));
			AddServices(builder);
			builder.Services.AddHttpContextAccessor();
		}

		public static void AddServices(WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
			builder.Services.AddScoped<ISenhaHasher, SenhaHasher>();
			builder.Services.AddScoped<ITokenGerador, TokenGerador>();
			builder.Services.AddScoped<IUsuarioService, UsuarioService>();
			builder.Services.AddScoped<ITarefasRepositorio, TarefasRepositorio>();
			builder.Services.AddScoped<ITarefasService, TarefasService>();
			builder.Services.AddScoped<ICheckItemRepositorio, CheckItemRepositorio>();
			builder.Services.AddScoped<ICheckItemService, CheckItemService>();
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
