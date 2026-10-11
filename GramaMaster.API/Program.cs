using FluentValidation;
using GramaMaster.API.ExceptionHandler;
using GramaMaster.API.Extensions;
using GramaMaster.Application.Extensions;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Security;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Services;
using GramaMaster.Application.Validators.Auth;
using GramaMaster.Infrastructure.Data;
using GramaMaster.Infrastructure.Persistence.Repositories;
using GramaMaster.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace GramaMaster.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterStudentValidator).Assembly);
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

        // Persistence
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Security / Infrastructure
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        // Application Services
        builder.Services.AddScoped<IAIService, AiService>();
        builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IContestService, ContestService>();
        builder.Services.AddScoped<ICurriculumService, CurriculumnService>();
        builder.Services.AddScoped<IExamService, ExamService>();
        builder.Services.AddScoped<IGrammarRuleService, GrammarRuleService>();
        builder.Services.AddScoped<IPracticeService, PracticeService>();
        builder.Services.AddScoped<IProblemService, ProblemService>();
        builder.Services.AddScoped<IStudentService, StudentService>();
        builder.Services.AddScoped<ITeacherService, TeacherService>();
        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<ITopicService, TopicService>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        app.UseApplicationMiddlewares();

        app.Run();
    }
}