
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

            var app = builder.Build();
     

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.AddApiDocumentation();

            app.MapControllers();

            app.Run();
        }
    }
}
