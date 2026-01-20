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

var builder = WebApplication.CreateBuilder(args);

var corsPolicyName = "AllowAngularDev";

// CORS ~ Cross-Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnswerNowDbContext>(options =>
{
    options.UseInMemoryDatabase("AnswerNowDb");
});

// JWT AUTHENTICATION CONFIGURATION
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //validate issuer
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        //validate audience
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        //validate signing key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),

        //validate expiration
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero //no tolerance for expiration smokey!

    };
});


//Register Services
builder.Services.AddScoped<IAdminService,  AdminService>();
builder.Services.AddScoped<IModeratorService, ModeratorService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();


// ENVIRONMENT
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
