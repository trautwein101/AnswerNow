using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Services;
using AnswerNow.Utilities.Extensions;
using AnswerNow.Data;
using Microsoft.EntityFrameworkCore;
using AnswerNow.Data.Repositories;
using AnswerNow.Data.IRepositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// -----------------------------
// Environments
// -----------------------------
builder.Configuration
    .AddJsonFile(
        $"environments/appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true);

// -----------------------------
// CORS ~ Cross-Origin Resource Sharing
// -----------------------------
const string corsPolicyName = "FrontendCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        var origins = builder.Configuration
            .GetSection("App:FrontendOrigins")
            .Get<string[]>() ?? Array.Empty<string>();

        if (builder.Environment.IsEnvironment("DEV") || builder.Environment.IsDevelopment())
        {
           if(origins.Length ==0)
            {
                origins = new[] { "http://localhost:4200" };
            }
        }

        if(origins.Length > 0)
        {
            policy
               .WithOrigins(origins)
               .AllowAnyHeader()
               .AllowAnyMethod();
        }
    });
});


// -----------------------------
// Controllers + Swagger
// -----------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// -----------------------------
// Health checks
// -----------------------------
builder.Services.AddHealthChecks();

// -----------------------------
// DB (PostgreSQL with Entity Framework Core)
// -----------------------------
builder.Services.AddDbContext<AnswerNowDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Missing ConnectionStrings:Default configuration.");

    options.UseNpgsql(connectionString);
});

// -----------------------------
// JWT Authentication Configuration
// -----------------------------
var jwtSecret = builder.Configuration["Jwt:SecretKey"];

//note: During EF design-time migration, supress JWT configuration to avoid crashing tooling and CI pipelines.
var isEfDesignTime = AppDomain.CurrentDomain.FriendlyName.Contains("ef", StringComparison.OrdinalIgnoreCase);

if (string.IsNullOrWhiteSpace(jwtSecret) && !isEfDesignTime)
{
    throw new InvalidOperationException(
        "JWT secret key is missing? Please setup configuration.");
}

if (!string.IsNullOrWhiteSpace(jwtSecret))
{

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)
            ),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero //no tolerance for expiration smokey!

        };
    });
}

// -----------------------------
// Register Services
// -----------------------------
builder.Services.AddScoped<IAdminService,  AdminService>();
builder.Services.AddScoped<IModeratorService, ModeratorService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// -----------------------------
// ENVIRONMENT
// -----------------------------
if (app.Environment.IsEnvironment("DEV") || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();


// WebApplicationFactory<Program> with minimal hosting
public partial class Program { }

