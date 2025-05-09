
using backend.Infraestrutura;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.AddDependecies();
            builder.AddJwtAuthentication();
            var app = builder.Build();
     

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.AddApiDocumentation();

            app.MapControllers();

            app.Run();
        }
    }
}
