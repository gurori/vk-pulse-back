using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using DataAccess;
using DataAccess.Repositories;
using Infastructure.Auth;
using Infrastructure.Auth;
using Infrastructure.Mapping;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000/");
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// DI Containers

// Repositoties
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IUserRepository, UserRepository>();

// Services
services.AddScoped<IUserService, UserService>();

// Auth
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

services.AddAutoMapper(typeof(UserAutoMapperProfile));

services.AddAuthentication(configuration);

services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)))
);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.EnsureCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
    }
);

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
