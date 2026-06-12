using System.Text;
using Medora.Application;
using Medora.Application.Common.Interfaces;
using Medora.Api.Security;
using Medora.Api.Services;
using Medora.Application.Modules.Identity.Auth;
using Medora.Infrastructure;
using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddDbContextCheck<MedoraDbContext>("sqlserver");

var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["SigningKey"];
if (!string.IsNullOrWhiteSpace(signingKey))
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
            };
        });
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MedoraSuperAdmin", policy => policy.RequireRole("MSA"));
    options.AddPolicy("TenantUserAdmin", policy => policy.RequireRole("SA", "HR"));
});

const string corsPolicy = "MedoraWeb";
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? throw new InvalidOperationException("CORS allowed origins are not configured.");

    options.AddPolicy(corsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await InitializeDatabaseAsync(app);

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    var databaseSection = app.Configuration.GetSection("Database");
    var applyMigrations = databaseSection.GetValue("ApplyMigrationsOnStartup", false);
    var seedOnStartup = databaseSection.GetValue("SeedOnStartup", false);

    if (!applyMigrations && !seedOnStartup)
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MedoraDbContext>();

    if (applyMigrations)
    {
        await dbContext.Database.MigrateAsync();
    }

    if (seedOnStartup)
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}

public partial class Program;
