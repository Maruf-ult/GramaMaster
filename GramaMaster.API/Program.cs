using FluentValidation;
using GramaMaster.API.ExceptionHandler;
using GramaMaster.API.Extensions;
using GramaMaster.Application.Extensions;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GramaMaster.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddSwaggerDocumentation();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddAutoMapperServices();

        builder.Services.AddDbContext<GramaMasterDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DbCon"));
        });

        var app = builder.Build();

        app.UseApplicationMiddlewares();

        app.Run();
    }
}