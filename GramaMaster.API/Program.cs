using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GramaMaster.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<GramaMasterDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbCon"));
            }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
