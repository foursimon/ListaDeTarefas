
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
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            var app = builder.Build();
     

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.AddApiDocumentation();

            app.MapControllers();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
