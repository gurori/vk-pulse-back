// Server/Program.cs
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Core.Configuration;
using DataAccess;
using DataAccess.Repositories;
using Infrastructure.Auth;
using Infrastructure.Mapping;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Controllers; // Для BaseController
using Server.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Конфигурация Options
services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000"); // Укажите ваш фронтенд-адрес
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// DI Containers

// Repositories
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ITasksRepository, TasksRepository>();

// Services
services.AddScoped<IUserService, UserService>();
services.AddScoped<ITaskService, TaskService>();

// Auth
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

// AutoMapper: сканируем сборки для профилей
services.AddAutoMapper(
    Assembly.GetExecutingAssembly(), // Current assembly (Server)
    typeof(UserAutoMapperProfile).Assembly // Infrastructure assembly
);

// Добавляем аутентификацию и авторизацию
services.AddAuthentication(configuration);

services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>(); // Добавляем фильтр исключений
});

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)))
);

var app = builder.Build();

// Применение миграций или создание БД
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    
    // Для разработки: создает базу данных, если ее нет, и заполняет сид-данными
    // В продакшене используйте MigrateAsync() после миграций
    // await dbContext.Database.EnsureCreatedAsync(); 
    
    // Для продакшена или если вы используете миграции
    await dbContext.Database.MigrateAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

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
