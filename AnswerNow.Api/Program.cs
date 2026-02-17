using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Services;
using AnswerNow.Data;
using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Repositories;
using AnswerNow.Utilities.Extensions;

var builder = WebApplication.CreateBuilder(args);
var isLambda = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"));

// -----------------------------
// AWS Cloudwatch logging
// -----------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// -----------------------------
// Environments
// -----------------------------
builder.Configuration
    .AddJsonFile(
        $"Environments/appsettings.{builder.Environment.EnvironmentName}.json",
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
            .Get<string[]>();

        // Fallback if config missing (safe defaults)
        if (origins == null || origins.Length == 0)
        {
            origins = new[]
            {
                "http://localhost:4200",
                "https://answernowplace.com"
                // later: "https://www.answernowplace.com"
            };
        }

        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetPreflightMaxAge(TimeSpan.FromHours(1));
    });
});


// -----------------------------
// Controllers + Swagger + AWS
// -----------------------------
builder.Services.AddControllers();

//note: run behind api gateway HTTP API once hosted in AWS Lambda
if (isLambda)
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
}

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

var connectionString = builder.Configuration.GetConnectionString("Default");
var healthChecks = builder.Services.AddHealthChecks();

//reflects “no DB configured”
healthChecks.AddCheck("ready-self", () => HealthCheckResult.Healthy(), tags: new[] { "ready" });

//note: only add postgres readiness check after db is configured for app boot in AWS before RDS exists.
if (!string.IsNullOrWhiteSpace(connectionString))
{
    healthChecks.AddNpgSql(
        connectionString,
        name: "postgres",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "ready" });
}

// -----------------------------
// DB (PostgreSQL with Entity Framework Core)
// -----------------------------

//note: only register DbContext when DB is configured after we set ConnectionStrings__Default in Lambda and it becomes active.
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<AnswerNowDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(5);
        });
    });
}

// -----------------------------
// JWT Authentication Configuration
// -----------------------------
var jwtSecret = builder.Configuration["Jwt:SecretKey"];

//note: During EF design-time migration, suppress JWT configuration to avoid crashing tooling and CI pipelines.
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
// DB EF Migrations ~ DEV (AWS RDS bootstrap)
// -----------------------------
if(!string.IsNullOrWhiteSpace(connectionString) && (app.Environment.IsEnvironment("DEV") || app.Environment.IsDevelopment()))
{
    using var scope = app.Services.CreateScope();

    //Resolve DbContext from DI and migrate it...
    var db = scope.ServiceProvider.GetRequiredService<AnswerNowDbContext>();
    db.Database.Migrate();
}

// -----------------------------
// ENVIRONMENT
// -----------------------------
if (app.Environment.IsEnvironment("DEV") || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

if (!isLambda)
{
    app.UseHttpsRedirection();
}

app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live");

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready")
});


app.MapControllers();

app.Run();


// WebApplicationFactory<Program> with minimal hosting
public partial class Program { }

